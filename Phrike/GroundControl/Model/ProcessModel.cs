using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Model
{
    static class ProcessModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void StartProcess(string cmdPath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = Environment.CurrentDirectory + cmdPath
                    }
                };
                process.Start();   
            }
            catch (Win32Exception e)
            {
                Logger.Error("Message: " + e.Message);
                Logger.Error("ErrorCode: " + e.ErrorCode);
                Logger.Error("StackTrace: " + e.StackTrace);
                Logger.Error("Source: " + e.Source);
                Logger.Error("GetBaseException Message: " + e.GetBaseException().Message);
            }
        }
    }
}
