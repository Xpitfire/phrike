using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Helper
{
    static public class PathHelper
    {
        /*
         * %localappdata%
         *  -> OperationPhrike
         *      -> Data (database)
         *      -> Picture (subject avatars)
         *      -> Import (also constains videos)
         */
        private static string LocalAppData => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string PhrikeDirectory => Path.Combine(LocalAppData, "OperationPhrike");

        public static string PhrikeData => Path.Combine(PhrikeDirectory, "Data");

        public static string PhrikePicture => Path.Combine(PhrikeDirectory, "Picture");

        public static string PhrikeImport => Path.Combine(PhrikeDirectory, "Import");

        public static string PhrikeLog => Path.Combine(PhrikeDirectory, "Log");

        public static string PhrikeScenario => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UnrealData");

        static PathHelper()
        {
            InitFileStorage();
        }

        private static void InitFileStorage()
        {
            CreateIfNotExisting(PhrikeDirectory);
            CreateIfNotExisting(PhrikeData);
            CreateIfNotExisting(PhrikePicture);
            CreateIfNotExisting(PhrikeImport);
            CreateIfNotExisting(PhrikeLog);
            //CreateIfNotExisting(PhrikeScenario); // Should exist
        }

        private static void CreateIfNotExisting(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string GetPicturePath(string name) => Path.Combine(PhrikePicture, name);

        public static string GetImportPath(string name) => Path.Combine(PhrikeImport, name);
    }
}
