using DataModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Phrike.GroundControl.Helper
{
    public class ScreenCaptureHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static ScreenCaptureHelper screenRercorder;
        private Process gameProcess;
        private Process cameraProcess;

        private const string DefaultGameRecordingFileName = "gameRecording.mkv";
        private const string DefaultWebcamRecordingFileName = "cameraRecording.mkv";

        public Boolean IsRunningGame { get; set; }
        public Boolean IsRunningCamera { get; set; }
        public String CameraConfig { get; set; }
        public String GameConfig { get; set; }

        private ScreenCaptureHelper()
        {
            // Private singleton constructor
            LoadConfig();
            IsRunningGame = false;
            IsRunningCamera = false;
        }

        public static ScreenCaptureHelper GetInstance()
        {
            if (screenRercorder == null)
            {
                //Create new instance if not available
                screenRercorder = new ScreenCaptureHelper();
            }
            return screenRercorder;
        }

        public void LoadConfig()
        {
            GameConfig = Settings.RecordingGameConfig;
            CameraConfig = Settings.RecordingCameraConfig;
        }

        public void StartRecording(int testId)
        {
            StartGameRecording(testId);
            StartCameraRecording(testId);
        }

        public void StartCameraRecording(int testId, String cameraFilename = DefaultWebcamRecordingFileName)
        {
            if (IsRunningCamera)
            {
                return;
            }

            if (StartProcessTask(ref cameraProcess, CameraConfig, cameraFilename, testId))
            {
                IsRunningCamera = true;
                Console.WriteLine(cameraProcess.Id);
            }
            else
            {
                DialogHelper.ShowErrorDialog("Bewegtbildaufzeichnungsgerätaufnahme konnte nicht gestartet werden. Externes Aufnahmeprogramm wurde nicht gefunden.");
        }
        }

        public void StartGameRecording(int testId, String gameFilename = DefaultGameRecordingFileName)
        {
            if (IsRunningGame)
            {
                return;
            }

            if (StartProcessTask(ref gameProcess, GameConfig, gameFilename, testId))
            {
                IsRunningGame = true;
                Console.WriteLine(gameProcess.Id);
            }
            else
            {
                DialogHelper.ShowErrorDialog("Simulationsaufnahme konnte nicht gestartet werden. Externes Aufnahmeprogramm wurde nicht gefunden.");
            }

        }

        private bool StartProcessTask(ref Process process, String config, String filename, int testId)
        {
            try
            {
                var aux = FileStorageHelper.ReserveFile(filename, AuxiliaryDataMimeTypes.AnyVideo, testId, DateTime.Now);
                String command = config + " \"" + PathHelper.PhrikeImport + "\\" + aux.FilePath + "\"";
                process = new Process();
                process.StartInfo.FileName = "ffmpeg.exe";
                process.StartInfo.Arguments = command;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                bool started = process.Start();
                process.WaitForExit(100);
                return started;
            }
            catch (System.ComponentModel.Win32Exception e)
            {                
                Logger.Error(e, $"Recording could not be started (config: {config}/ filename: {filename}).");
                return false;
            }
        }

        public void StopRecording()
        {
            StopGameRecording();
            StopCameraRecording();
        }

        public void StopCameraRecording()
        {
            if (IsRunningCamera)
            {
                if (StopProcess(ref cameraProcess))
                {
                    IsRunningCamera = false;
                }
            }
        }

        public void StopGameRecording()
        {
            if (IsRunningGame)
            {
                if (StopProcess(ref gameProcess))
                {
                    IsRunningGame = false;
                }
            }
        }

        private bool StopProcess(ref Process process)
        {
            Process stopProcess = new Process();
            stopProcess.StartInfo.FileName = "SendSignalCtrlC.exe";
            stopProcess.StartInfo.Arguments = process.Id.ToString();
            stopProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            bool stopped = false;
            while (!process.HasExited)
            {
                stopped = stopProcess.Start();
                stopProcess.WaitForExit(100);
            }
            return stopped;
        }
    }
}
