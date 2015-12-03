// <summary>Implementation of the FileStorage class.</summary>
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
    /// <summary>
    /// Helper class that should be used for importing files / <see cref="AuxilaryData"/>.
    /// </summary>
    public static class FileStorageHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string GetTargetPath(string src, int id, string dstDir)
                => Path.Combine(dstDir, id + "-" + Path.GetFileName(src));

        // Note that this may generate false negatives. See
        // http://stackoverflow.com/a/410794/2128694 if we ever need a proper implementation.
        private static bool IsFileInDir(string filepath, string dir) =>
            string.Compare(
                Path.GetDirectoryName(Path.GetFullPath(filepath)),
                dir,
                StringComparison.InvariantCultureIgnoreCase) == 0;

        /// <summary>
        /// Import a file into the Phrike file storage directory and create
        /// and return a new <see cref="AuxilaryData"/> object in the database.
        /// </summary>
        /// <param name="fromPath">
        /// The path from which to import the file.
        /// May not already point to the Phrike file storage directory!
        /// </param>
        /// <param name="mimeType">The value for <see cref="AuxilaryData.MimeType"/>.</param>
        /// <param name="testId">
        /// The ID of the test the file is associated with (see <see cref="AuxilaryData.Test"/>).
        /// </param>
        /// <param name="timestamp">
        /// The value for <see cref="AuxilaryData.Timestamp"/> (optional).
        /// Defaults to the creation time of the file at <see cref="fromPath"/>.
        /// </param>
        /// <param name="description">
        /// The value for <see cref="AuxilaryData.Description"/> (optional).
        /// </param>
        /// <returns>
        /// A newly created <see cref="AuxilaryData"/> object, already inserted in the database.
        /// </returns>
        public static AuxilaryData ImportFile(
            [NotNull] string fromPath,
            [NotNull] string mimeType,
            int testId,
            DateTime? timestamp = null,
            string description = null)
        {
            Logger.Trace($"Importing file {fromPath} ({mimeType}).");
            if (IsFileInDir(fromPath, PathHelper.PhrikeImport))
            {
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
                string targetPath = GetTargetPath(fromPath, aux.Id, PathHelper.PhrikeImport);
                File.Copy(fromPath, targetPath);
                try
                {
                    aux.FilePath = Path.GetFileName(targetPath);
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

        /// <summary>
        /// Imports fromPath into the picture storage and sets it as the
        /// subjects avatar. If <paramref name="db"/> is not null, also saves
        /// the subject to the DB.
        /// </summary>
        public static void SetSubjectAvatar([NotNull] string fromPath, [NotNull] Subject subject, UnitOfWork db = null)
        {
            Logger.Trace($"Importing picture {fromPath}.");
            if (IsFileInDir(fromPath, PathHelper.PhrikePicture))
            {
                throw new ArgumentException(@"Picture is already in the picture storage.", nameof(fromPath));
            }
            string targetPath = GetTargetPath(
                fromPath,
                subject.Id,
                PathHelper.PhrikePicture);
            string oldPath = subject.AvatarPath;
            string restorePath = null;
            if (!string.IsNullOrEmpty(oldPath))
            {
                restorePath = Path.GetTempFileName();
                File.Move(PathHelper.GetPicturePath(oldPath), restorePath);
            }
            try
            {
                File.Copy(fromPath, targetPath);
                subject.AvatarPath = Path.GetFileName(targetPath);
                db?.Save();
            }
            catch
            {
                if (restorePath != null)
                {
                    File.Move(restorePath, oldPath);
                }
                subject.AvatarPath = oldPath;
                throw;
            }
            Logger.Info($"Sucessfully imported picture {fromPath} as {targetPath}.");
        }
    }
}