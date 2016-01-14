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

        public static AuxilaryData ReserveFile(
            [NotNull] string name,
            [NotNull] string mimeType,
            int testId,
            DateTime? timestamp, string description = null)
        {
            Logger.Trace($"Reserving file {name} ({mimeType}).");
            if (name != Path.GetFileName(name))
            {
                throw new ArgumentException("Name must not contain directory.", nameof(name));
            }

            using (var ts = new TransactionScope())
            using (var db = new UnitOfWork())
            {
                var aux = new AuxilaryData
                {
                    FilePath = name,
                    Test = db.TestRepository.GetByID(testId),
                    Timestamp = timestamp ?? DateTime.Now,
                    MimeType = mimeType,
                    Description = description ?? name
                };
                db.AuxiliaryDataRepository.Insert(aux);
                db.Save(); // Save to generate ID.
                aux.FilePath = aux.Id + "-" + name;
                db.Save();
                ts.Complete();
                Logger.Info(
                    $"Sucessfully resved file {name} ({mimeType}) as {aux.FilePath}.");
                return aux;
            }
        }

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
                throw new ArgumentException("File is already in the file storage.", nameof(fromPath));
            }

            using (var ts = new TransactionScope())
            using (var db = new UnitOfWork())
            {
                AuxilaryData aux = ReserveFile(
                    Path.GetFileName(fromPath),
                    mimeType,
                    testId,
                    timestamp ?? File.GetCreationTime(fromPath),
                    description);
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
        public static void SetSubjectAvatar(string fromPath, [NotNull] Subject subject, UnitOfWork db = null)
        {
            if (!string.IsNullOrWhiteSpace(fromPath))
            {
                Logger.Trace($"Importing picture {fromPath}.");
                if (IsFileInDir(fromPath, PathHelper.PhrikePicture))
                {
                    throw new ArgumentException(
                        @"Picture is already in the picture storage.",
                        nameof(fromPath));
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
                    try
                    {
                        File.Delete(restorePath); // Dirty hack
                        File.Move(PathHelper.GetPicturePath(oldPath), restorePath);
                    }
                    catch (Exception e)
                    {
                        Logger.Warn(e, $"Failed moving old picture from {oldPath} to {restorePath}.");
                        restorePath = null;
                        try
                        {
                            File.Delete(PathHelper.GetPicturePath(oldPath));
                        }
                        catch (Exception e2)
                        {
                            Logger.Warn(e2, $"Also failed just deleting old picture from {oldPath}.");
                        }
                    }
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
                if (!string.IsNullOrEmpty(restorePath))
                {
                    File.Delete(restorePath);
                }
                Logger.Info($"Sucessfully imported picture {fromPath} as {targetPath}.");
            }
            else if (subject.AvatarPath != null)
            {
                Logger.Trace($"Removing picture for Subject {subject.Id}.");
                string path = subject.AvatarPath;
                using (var ts = new TransactionScope())
                {
                    subject.AvatarPath = null;
                    db?.Save();
                    File.Delete(PathHelper.GetPicturePath(path));
                    ts.Complete();
                }
            }
        }

        public static void DeleteFile(int auxDataId)
        {
            Logger.Trace($"Deleting file #{auxDataId}.");

            using (var ts = new TransactionScope())
            using (var db = new UnitOfWork())
            {
                AuxilaryData data = db.AuxiliaryDataRepository.GetByID(auxDataId);
                string path = data.FilePath;
                db.AuxiliaryDataRepository.Delete(data);
                db.Save();
                File.Delete(PathHelper.GetImportPath(path));
                ts.Complete();
                Logger.Info($"Sucessfully deleted file {path}.");
            }
        }
    }
}