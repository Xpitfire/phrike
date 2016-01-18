using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using NLog;
using System.IO;

namespace Phrike.GroundControl.Controller
{
    /// <summary>
    /// This is used to start and stop sub-processes.
    /// </summary>
    static class ProcessController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<string, Process> ProcesseDictionary = new Dictionary<string, Process>(); 

        public static void StartProcess(string cmdPath, bool useRelativePath = true, string[] cmdParams = null)
        {
            if (cmdPath == null)
            {
                Logger.Warn("Could not start invalid process!");
                return;
            }
            if (ProcesseDictionary.ContainsKey(cmdPath) && !ProcesseDictionary[cmdPath].HasExited)
            {
                Logger.Warn("Could not start process! Process already exists.");
                return;
            }

            try
            {
                //var process = new Process
                //{
                //    StartInfo =
                //    {
                        
                //        WindowStyle = ProcessWindowStyle.Hidden,
                //        FileName = (useRelativePath) ? Path.Combine(Environment.CurrentDirectory, cmdPath) : cmdPath,
                //        Arguments = (cmdParams != null) ? String.Join(" ", cmdParams) : ""
                //    }
                //};
                Process process = new Process()
                             {
                                 StartInfo =
                                 {
                                     FileName = SettingsController.UEPath,
                                     Arguments = $"{cmdPath} -game {String.Join(" ", cmdParams)}",
                                     UseShellExecute = false
                                 }
                             };
                //// TODO: Verify if process not started
                process.Start();
                ProcesseDictionary[cmdPath] = process;
                Logger.Info("New process started: {0}", cmdPath);
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

        public static void StopProcess(string cmdPath)
        {
            if (cmdPath == null)
            {
                Logger.Warn("Could not stop invalid process!");
                return;
            }
            Process process = null;
            if (ProcesseDictionary.ContainsKey(cmdPath))
                process = ProcesseDictionary[cmdPath];
            if (process != null)
            {
                if (!process.HasExited)
                {
                    process.Close();
                }
                ProcesseDictionary.Remove(cmdPath);
                Logger.Info("Stopped process: {0}", cmdPath);
            }
            else
            {
                Logger.Warn("No process found to dispose!");
            }
        }
    }
}
