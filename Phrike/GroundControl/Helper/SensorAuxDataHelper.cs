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

        /// <summary>
        /// Read the file specified by <paramref name="aux"/> and convert it into a <see cref="DataBundle"/>.
        /// </summary>
        /// <param name="aux">
        /// The file to read. Must have a supported MIME type.
        /// </param>
        /// <returns>
        /// A new <see cref="DataBundle"/> with the data from <paramref name="aux"/>.
        /// </returns>
        public static DataBundle AuxDataToSensorData(AuxilaryData aux)
        {
            Logger.Info($"Attempting to open {aux.FilePath} ({aux.MimeType}) as sensor data.");
            ISensorHub hub = null;
            try
            {
                if (aux.MimeType == AuxiliaryDataMimeTypes.GMobilabPlusBin)
                {
                    hub = new GMobiLab.SensorDataFileStreamer(PathHelper.GetImportPath(aux.FilePath));
                }
                else if (aux.MimeType == AuxiliaryDataMimeTypes.Biofeedback2000Csv)
                {
                    hub = new BiofeedbackCsvFileStreamer(PathHelper.GetImportPath(aux.FilePath));
                }
                else
                {
                    const string Message = "Unknown sensor data mime type.";
                    Logger.Error(Message);
                    throw new ArgumentException(Message, nameof(aux));
                }
                Logger.Trace("File opened, loading...");
                DataBundle bundle = DataBundle.FromHub(hub);
                Logger.Trace("File sucessfully loaded.");
                return bundle;
            }
            finally
            {
                hub?.Dispose();
            }
        }

        /// <summary>
        /// Import a sensor data file for the specified test.
        /// </summary>
        /// <param name="fpath">
        /// The path to a sensor data file. Must have a supported file name extension.
        /// </param>
        /// <param name="testId">
        /// The ID of the test to which the file belongs.
        /// </param>
        /// <returns>
        /// A newly created <see cref="AuxilaryData"/> object already contained in the database.
        /// </returns>
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