using Phrike.GroundControl.Model;
using System;
using System.Threading.Tasks;
using NLog;

namespace Phrike.GroundControl.ViewModels
{

    class StressTestViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const string UnrealEnginePath = @"C:\public\OperationPhrike\Phrike\GroundControl\UnrealData\Balance.exe";
        private static StressTestViewModel instance;

        private SensorsModel sensorsModel;
        private UnrealEngineModel unrealEngineModel;

        public StressTestViewModel()
        {
            instance = this;
        }

        public static StressTestViewModel GetInstance()
        {
            return instance;
        }

        public void StartUnrealEngine()
        {
            StartUnrealEngineTask();
        }
        public void StopUnrealEngine()
        {
            StopUnrealEngineTask();
        }
        public void StartSensors()
        {
            StartSensorsTask();
        }
        public void StopSensors()
        {
            StopSensorsTask();
        }
        public void StartScreenCapture()
        {
            StartScreenCaptureTask();
        }
        public void StopScreenCapture()
        {
            StopScreenCaptureTask();
        }

        public async void ApplicationClose()
        {
            await ApplicationCloseTask();
        }

        public async void AutoStressTest()
        {
            await StartUnrealEngineTask();
            await StartScreenCaptureTask();
            await StartSensorsTask();
        }

        public async Task StartUnrealEngineTask()
        {
            await Task.Run(() =>
            {
                ProcessModel.StartProcess(UnrealEnginePath, false);
                Logger.Info("Unreal Engine process started!");
                unrealEngineModel = new UnrealEngineModel();
                Logger.Info("Unreal Engine is ready to use!");
            });
        }

        public async Task StopUnrealEngineTask()
        {
            await Task.Run(() =>
            {
                if (unrealEngineModel == null)
                {
                    Logger.Warn("Could not stop the Unreal Engine! No Unreal Engine instance active!");
                    return;
                }

                unrealEngineModel.Close();
                unrealEngineModel = null;
                ProcessModel.StopProcess(UnrealEnginePath);
                Logger.Info("Unreal Engine process stoped!");
            });
        }

        public async Task StartSensorsTask()
        {
            await Task.Run(() =>
            {
                if (sensorsModel != null)
                    sensorsModel.Close();

                sensorsModel = new SensorsModel();
                Logger.Info("Sensors instance created!");
                sensorsModel.StartRecording();
            });
        }

        public async Task StopSensorsTask()
        {
            await Task.Run(() =>
            {
                if (sensorsModel == null)
                {
                    Logger.Warn("Could not stop sensors recording! No sensors recording instance enabled!");
                    return;
                }
                sensorsModel.Close();
                sensorsModel = null;
                Logger.Info("Sensors recording successfully stopped!");
            });
        }

        public async Task StartScreenCaptureTask()
        {
            if (unrealEngineModel != null)
            {
                await Task.Run(() =>
                {
                    unrealEngineModel.StartCapture();
                    Logger.Info("Screen Capture successfully started!");
                });
            }
        }
        public async Task StopScreenCaptureTask()
        {
            if (unrealEngineModel != null)
            {
                await Task.Run(() =>
                {
                    unrealEngineModel.StopCapture();
                    Logger.Info("Screen Capture successfully stopped!");
                });
            }
        }

        public async Task ApplicationCloseTask()
        {
            await StopSensorsTask();
            await StopScreenCaptureTask();
            await StopUnrealEngineTask();
        }

    }
}
