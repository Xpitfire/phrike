// <summary></summary>
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
using System.Transactions;

using DataAccess;

using DataModel;

using NLog;

using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.Helper
{
    public static class FileStorageHelper
    {
        private static readonly string StoragePath = PathHelper.PhrikeImport;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static AuxilaryData ImportFile(
            [NotNull] string fromPath,
            [NotNull] string mimeType,
            int testId,
            DateTime? timestamp = null,
            string description = null)
        {
            Logger.Trace($"Importing file {fromPath} ({mimeType}).");
            fromPath = Path.GetFullPath(fromPath);
            if (String.Compare(
                Path.GetDirectoryName(fromPath),
                StoragePath,
                StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // Note that this may generate false negatives. See
                // http://stackoverflow.com/a/410794/2128694 if we ever need a proper implementation.
                throw new ArgumentException(@"File is already in the file storage.", nameof(fromPath));
            }
            using (var ts = new TransactionScope())
            using (var db = new UnitOfWork())
            {
                var aux = new AuxilaryData
                {
                    FilePath = fromPath,
                    Test = db.TestRepository.GetByID(testId),
                    Timestamp = timestamp ?? File.GetCreationTime(fromPath),
                    MimeType = mimeType,
                    Description = description
                };
                db.AuxiliaryDataRepository.Insert(aux);
                db.Save(); // Save to generate ID.
                string targetPath = Path.Combine(
                    StoragePath,
                    aux.Id + "-" + Path.GetFileName(fromPath));
                File.Copy(fromPath, targetPath);
                try
                {
                    aux.FilePath = targetPath;
                    db.Save();
                    ts.Complete();
                }
                catch
                {
                    File.Delete(targetPath);
                    throw;
                }
                Logger.Info(
                    $"Sucessfully imported file {fromPath} ({mimeType}) as {targetPath}.");
                return aux;

            }
        }
    }
}