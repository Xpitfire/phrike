using Phrike.GroundControl.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Phrike.PhrikeScreenCapture
{
    public class PhrikeScreenCapture
    {
        private static PhrikeScreenCapture screenRercorder;
        private Process gameProcess;
        private Process cameraProcess;

        public Boolean IsRunningGame { get; set; }
        public Boolean IsRunningCamera { get; set; }
        public String CameraConfig { get; set; }
        public String GameConfig { get; set; }

        private PhrikeScreenCapture()
        {
            // Private singleton constructor
            LoadConfig(PathHelper.PhrikeData + "\\config.txt");
            IsRunningGame = false;
            IsRunningCamera = false;
        }

        public static PhrikeScreenCapture GetInstance()
        {
            if (screenRercorder == null)
            {
                //Create new instance if not available
                screenRercorder = new PhrikeScreenCapture();
            }
            return screenRercorder;
        }

        public void LoadConfig(string configFile)
        {
            string[] lines = System.IO.File.ReadAllLines(configFile);
            if (lines.Count() >= 2)
            {
                GameConfig = lines[0];
                CameraConfig = lines[1];
            }
        }

        public void StartRecording(String directory, String gameFilename, String cameraFilename, int testId)
        {
            StartGameRecording(directory, gameFilename, testId);
            StartCameraRecording(directory, cameraFilename, testId);
        }

        public void StartCameraRecording(String directory, String cameraFilename, int testId)
        {
            if (IsRunningCamera)
            {
                return;
            }

            if (StartProcessTask(ref cameraProcess, CameraConfig, directory, cameraFilename, testId))
            {
                IsRunningCamera = true;
                Console.WriteLine(cameraProcess.Id);
            }
        }

        public void StartGameRecording(String directory, String gameFilename, int testId)
        {
            if (IsRunningGame)
            {
                return;
            }

            if (StartProcessTask(ref gameProcess, GameConfig, directory, gameFilename, testId))
            {
                IsRunningGame = true;
                Console.WriteLine(gameProcess.Id);
            }

        }

        private bool StartProcessTask(ref Process process, String config, String directory, String filename, int testId)
        {
            FileStorageHelper.ReserveFile(filename, ".mkv", testId, DateTime.Now);
            String command = config + " \"" + directory + filename + "\"";
            process = new Process();
            process.StartInfo.FileName = "ffmpeg.exe";
            process.StartInfo.Arguments = command;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            bool started = process.Start();
            Thread.Sleep(100);
            return started;
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
            bool started = stopProcess.Start();
            Thread.Sleep(100);
            return started;
        }
    }
}
