// Helper functions for gMobiLab Bluethoot data transfer.
// -----------------------------------------------------------------------
// Copyright (c) 2015 University of Applied Sciences Upper-Austria
// Project OperationPhrike
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------------------------

#include <boost/lockfree/spsc_queue.hpp>
#include <thread>

namespace lf = boost::lockfree;

#define NOMINMAX
#define WIN32_LEAN_AND_MEAN
#define STRICT
#include <Windows.h>
#include <gMOBIlabplus.h>

#define PHRIKE_XFER_API(r) extern "C" __declspec(dllexport) r __stdcall

const std::size_t MaxNChannels = 9;

// Enough for one minute when all channels are scanned.
const std::size_t QueueSize = SAMPLERATE * MaxNChannels * 60;

// Only for internal use.
class Phrike_XferThread
{
public:
    Phrike_XferThread(HANDLE hDevice)
        : m_hDevice(hDevice)
        , m_running(false)
        , m_destroySelf(false)
        , m_gMobiLabError(0)
        , m_winError(0)
    { }

    bool Start()
    {
        if (m_thread.joinable())
            return false;
        m_running = true;
        m_thread = std::thread(&Phrike_XferThread::XferThreadMain, this);
        return true;
    }

    bool IsRunning() const
    {
        return m_running;
    }

    bool JoinThread()
    {
        if (!m_thread.joinable())
            return false;
        m_thread.join();
        return true;
    }

    void GetData(DWORD* count, SHORT* data)
    {
        *count = m_data.pop(data, *count);
    }

    DWORD AvailableCount() const
    {
        return m_data.read_available();
    }

    DWORD DropData()
    {
        return m_data.consume_all([](SHORT){ /* Drop. */ });
    }

    void GetErrors(UINT* gMobiErr, DWORD* winErr)
    {
        BOOST_ASSERT(!m_running);
        if (gMobiErr)
            *gMobiErr = m_gMobiLabError;
        if (winErr)
            *winErr = m_winError;
    }

private:
    // Ends as soon as GT_GetData or GetOverlappedResult fails.
    // Keeps pushing the result of GT_GetData to m_data using double-buffering
    // so that m_data.push() is called while the async IO initiated by
    // GT_GetData runs.
    void XferThreadMain()
    {
        OVERLAPPED ov = {};
        _BUFFER_ST bufState = {};

        // Since we do not save time stamps yet, use maximum buffer size.
        static const DWORD BufSz = 1024;

        SHORT buf[BufSz][2];
        int writeBufIdx = 0;
        int readBufIdx = -1;
        DWORD readBufValidCount = 0;
        bufState.size = BufSz;
        ov.hEvent = CreateEventW(nullptr, FALSE, FALSE, nullptr);
        BOOST_ASSERT(ov.hEvent);

        while (!m_destroySelf) // While no error occured:
        {
            bufState.pBuffer = buf[writeBufIdx];
            if (!GT_GetData(m_hDevice, &bufState, &ov)) // Start transfer.
            {
                BOOST_VERIFY(GT_GetLastError(&m_gMobiLabError));
                break;
            }

            if (readBufIdx < 0) // First iteration: Nothing has been read.
            {
                readBufIdx = 1 - writeBufIdx;
            }
            else // Push previouly read data to m_data.
            {
                std::size_t nPushed = 0;
                while (!m_destroySelf) // Until all data has been pushed:
                {
                     nPushed += m_data.push(
                        buf[readBufIdx] + nPushed,
                        readBufValidCount - nPushed);
                     if (nPushed >= readBufValidCount)
                         break;
                     std::this_thread::yield();
                }

                std::swap(writeBufIdx, readBufIdx);
            }

            // Wait for asyncronous transfer to finish.
            if (!GetOverlappedResult(m_hDevice, &ov, &readBufValidCount, TRUE))
                break;
        }

        m_winError = GetLastError();
        BOOST_VERIFY(CloseHandle(ov.hEvent));

        if (m_destroySelf)
            delete this;
        else
            m_running = false;
    }

    lf::spsc_queue<SHORT, lf::capacity<QueueSize>> m_data;
    HANDLE m_hDevice;
    std::atomic<bool> m_running;
    std::atomic<bool> m_destroySelf;
    std::thread m_thread;
    UINT m_gMobiLabError;
    DWORD m_winError;

};

// Returns a new Xfer thread or NULL on error. Does not start the thread.
// You must free the thread with Phrike_DestroyXferThread.
PHRIKE_XFER_API(Phrike_XferThread*) Phrike_CreateXferThread(HANDLE hDevice)
{
    if (!hDevice)
        return nullptr;
    try
    {
        return new Phrike_XferThread(hDevice);
    }
    catch (std::exception const&)
    {
        return nullptr;
    }
}

// Starts the given Xfer thread. GT_StartAcquisition must have been called on
// the corresponding device handle, or the thread will stop immediately.
PHRIKE_XFER_API(BOOL) Phrike_StartXferThread(Phrike_XferThread* xfer)
{
    try
    {
        return xfer ? xfer->Start() : FALSE;
    }
    catch (std::exception const&)
    {
        return FALSE;
    }
}

// Destroy the xfer thread. This function returns FALSE if the thread is
// still running. However, the thread is then still marked for deletion
// and will delete itself on exit.
// Call GT_StopAquisition on the device handle corresponding to the thread
// and then get or drop all remaining data to stop it. Note that even then,
// some time may pass before the tread becomes destroyable. Call
// Phrike_JoinXferThread to wait on it.
PHRIKE_XFER_API(BOOL) Phrike_DestroyXferThread(Phrike_XferThread* xfer)
{
    if (!xfer)
        return TRUE;

    if (xfer->IsRunning())
        return FALSE;

    xfer->JoinThread();

    delete xfer;
    return TRUE;
}

// Waits for xfer to exit. Note that this may never happen if there is
// unconsumed data remaining or the data acquisition is still running.
PHRIKE_XFER_API(BOOL) Phrike_JoinXferThread(Phrike_XferThread* xfer)
{
    return xfer ? xfer->JoinThread() : FALSE;
}


// Returns whether the xfer thread has finished (is not running).
// The output parameters winErr and gMobiLabErr are only
// valid if TRUE was returned.
// Parameters:
//   xfer [In]: A Xfer thread as returned by Phrike_CreateXferThread.
//   gMobiLabErr [Out opt]: The result of GT_GetLastError.
//   gMobiLabErr [Out opt]: The result of Win32's GetLastError in the thread.
PHRIKE_XFER_API(BOOL) Phrike_XferThreadResult(
    Phrike_XferThread* xfer, UINT* gMobiLabErr, DWORD* winErr)
{
    if (!xfer || xfer->IsRunning())
        return FALSE;
    xfer->GetErrors(gMobiLabErr, winErr);
    return TRUE;
}

// Drop all data from an xfer thread. Useful before destroying.
// Returns how many SHORTs were dropped.
PHRIKE_XFER_API(DWORD) Phrike_DropXferData(Phrike_XferThread* xfer)
{
    return xfer ? xfer->DropData() : 0;
}

// Get the data collected so far by the Xfer thread.
// Parameters:
//   xfer [In]: A Xfer thread as returned by Phrike_CreateXferThread.
//   count [In/Out]: Must be set to the maximum number of SHORTs to copy to
//                   data. Upon successful return from this function, receives
//                   the number of SHORTs actually copied (can be less).
//   data [Out]: Must be set to an array of at least *count SHORTs. Upon
//               successful return, sensor data will be copied to it.
PHRIKE_XFER_API(BOOL) Phrike_GetXferData(
    Phrike_XferThread* xfer, DWORD* count, SHORT* data)
{
    if (!xfer || !count || !data)
        return FALSE;
    xfer->GetData(count, data);
    return TRUE;
}

// Returns how many SHORTs have been collected so far by xfer.
// Note that this value can increase between the call to this function and a
// call to Phrike_GetXferData.
PHRIKE_XFER_API(DWORD) Phrike_AvailableXferCount(Phrike_XferThread* xfer)
{
    return xfer ? xfer->AvailableCount() : 0;
}
