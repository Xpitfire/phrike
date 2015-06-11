using Phrike.GMobiLab;
using Phrike.GroundControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace Phrike.GroundControl.ViewModels
{

    class StressTestViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const string UnrealEnginePath = @"C:\public\OperationPhrike\Phrike\GroundControl\UnrealData\Balance.exe";

        private SensorDevice sensors;

        public async void StartUnrealEngine()
        {
            await Task.Run(() =>
            {
                ProcessModel.StartProcess(UnrealEnginePath, false);
                Logger.Info("Unreal Engine process started!");
            });
        }

        public async void StopUnrealEngine()
        {
            await Task.Run(() =>
            {
                ProcessModel.StopProcess(UnrealEnginePath);
                Logger.Info("Unreal Engine process stoped!");
            });
        }

        public async void StartSensors()
        {
            await Task.Run(() =>
            {
                if (sensors != null)
                {
                    Logger.Info("Sensors already started!");
                    return;
                }

                try
                {
                    sensors = new SensorDevice("COM7:");
                    sensors.SetSdFilename("gc-test");
                    sensors.StartRecording();
                    Logger.Info("Sensors recording started!");
                }
                catch (Exception e)
                {
                    Logger.Error("Could not connect to sensor device!", e.Message);
                }
            });
        }

        public async void StopSensors()
        {
            await Task.Run(() =>
            {
                if (sensors != null)
                {
                    sensors.StopRecording();
                    sensors = null;
                    Logger.Info("Sensors recording stopped!");
                }
                else
                {
                    Logger.Info("Sensors recording is not running!");
                }
            });
        }

        public async void StartScreenCapture()
        {
            await Task.Run(() =>
            {
                // TODO implementation
                Logger.Warn("Screen Capture currently not supported!");
            });
        }

    }
}
