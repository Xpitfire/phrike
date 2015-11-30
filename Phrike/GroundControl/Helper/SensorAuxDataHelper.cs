// <summary>Implements the SensorAuxDataHelper.</summary>
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

using System;
using System.IO;

using DataModel;

using NLog;

using Phrike.Sensors;

namespace Phrike.GroundControl.Helper
{
    public class SensorAuxDataHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static DataBundle AuxDataToSensorData(AuxilaryData aux)
        {
            Logger.Info($"Attempting to open {aux.FilePath} ({aux.MimeType}) as sensor data.");
            ISensorHub hub = null;
            try
            {
                if (aux.MimeType == AuxiliaryDataMimeTypes.GMobilabPlusBin)
                {
                    hub = new GMobiLab.SensorDataFileStreamer(aux.FilePath);
                }
                else if (aux.MimeType == AuxiliaryDataMimeTypes.Biofeedback2000Csv)
                {
                    hub = new BiofeedbackCsvFileStreamer(aux.FilePath);
                }
                else
                {
                    const string Message = "Unknown sensor data mime type.";
                    Logger.Error(Message);
                    throw new ArgumentException(Message, nameof(aux));
                }
                Logger.Trace("File opened.");
                return DataBundle.FromHub(hub);
            }
            finally
            {
                hub?.Dispose();
            }
        }

        public static AuxilaryData ImportSensorDataFile(string fpath, int testId)
        {
            return FileStorageHelper.ImportFile(fpath, GetMimeType(fpath), testId);
        }

        private static string GetMimeType(string fpath)
        {
            string ext = Path.GetExtension(fpath)?.ToLowerInvariant();
            if (ext == ".bin")
                return AuxiliaryDataMimeTypes.GMobilabPlusBin;
            if (ext == ".csv")
                return AuxiliaryDataMimeTypes.Biofeedback2000Csv;
            const string Message = "Unknown sensor data file extension";
            Logger.Error(Message);
            throw new ArgumentException(Message, nameof(fpath));
        }
    }
}