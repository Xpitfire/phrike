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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace DataModel
{
    /// <summary>
    /// Defines string constants for use as <see cref="AuxilaryData.MimeType"/>.
    /// </summary>
    public static class AuxiliaryDataMimeTypes
    {
        public const string Biofeedback2000Csv= "application/x-biofeedback200";
        public const string GMobilabPlusBin = "application/x-gmobilabplusbin";
        public const string AnyVideo = "video/x-video";

        public enum Category
        {
            Unknown,

            Video,

            SensorData
        };

        public static readonly IReadOnlyDictionary<string, string> FileExtMimeTypes =
            new Dictionary<string, string>
        {
            {"csv", Biofeedback2000Csv},
            {"bin", GMobilabPlusBin},
            {"mp4", AnyVideo},
            {"mkv", AnyVideo},
            {"avi", AnyVideo},
            {"wmv", AnyVideo},
        };

        public static string GetMimeTypeForPath(string path)
        {
            string ext = Path.GetExtension(path);
            if (ext == null || !ext.StartsWith("."))
            {
                throw new KeyNotFoundException("File has no extension.");
            }

            return FileExtMimeTypes[ext.Substring(1)];
        }

        public static Category GetCategory(string mimetype)
        {
            switch (mimetype)
            {
                case Biofeedback2000Csv: return Category.SensorData;
                case GMobilabPlusBin: return Category.SensorData;
                case AnyVideo: return Category.Video;
                default: return Category.Unknown;
            }
    }
    }
}