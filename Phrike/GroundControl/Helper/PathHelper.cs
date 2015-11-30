using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Helper
{
    static class PathHelper
    {
        /*
         * %localappdata%
         *  -> OperationPhrike
         *      -> Data
         *      -> Video
         *      -> Picture
         *      -> Import
         */
        private static string LocalAppData => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string PhrikeDirectory => Path.Combine(LocalAppData, "OperationPhrike");

        public static string PhrikeData => Path.Combine(PhrikeDirectory, "Data");

        public static string PhrikeVideo => Path.Combine(PhrikeDirectory, "Video");

        public static string PhrikePicture => Path.Combine(PhrikeDirectory, "Picture");

        public static string PhrikeImport => Path.Combine(PhrikeDirectory, "Import");

        public static string PhrikeScenario => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UnrealData");

        static PathHelper()
        {
            InitFileStorage();
        }

        private static void InitFileStorage()
        {
            CreateIfNotExisting(PhrikeDirectory);
            CreateIfNotExisting(PhrikeData);
            CreateIfNotExisting(PhrikeVideo);
            CreateIfNotExisting(PhrikePicture);
            CreateIfNotExisting(PhrikeImport);
            //CreateIfNotExisting(PhrikeScenario); // Should exist
        }

        private static void CreateIfNotExisting(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
