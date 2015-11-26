// <summary>Implements the SensorAuxDataController.</summary>
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

using DataAccess;

using DataModel;

using NLog;

using Phrike.Sensors;

namespace Phrike.GroundControl.Controller
{
    public class SensorAuxDataController
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

        public static AuxilaryData InsertSensorDataFile(string fpath, Test test)
        {
            Logger.Info("Attempting to add " + fpath + " as auxiliary data.");
            using (var db = new UnitOfWork())
            {
                var aux = new AuxilaryData
                {
                    FilePath = fpath,
                    Test = test,
                    Timestamp = File.GetCreationTime(fpath),
                    MimeType = GetMimeType(fpath)
                };
                db.AuxiliaryDataRepository.Insert(aux);
                Logger.Trace("Successfully inserted auxiliary data.");
                return aux;
            }
        }

        private static string GetMimeType(string fpath)
        {
            string ext = Path.GetExtension(fpath);
            if (ext == "bin")
                return AuxiliaryDataMimeTypes.GMobilabPlusBin;
            if (ext == "csv")
                return AuxiliaryDataMimeTypes.Biofeedback2000Csv;
            const string Message = "Unknown sensor data file extension";
            Logger.Error(Message);
            throw new ArgumentException(Message, nameof(fpath));
        }
    }
}