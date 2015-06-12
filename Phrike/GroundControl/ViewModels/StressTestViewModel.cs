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

        private SensorsModel sensorsModel;
        private UnrealEngineModel unrealEngineModel;

        public async Task StartUnrealEngine()
        {
            await Task.Run(() =>
            {
                ProcessModel.StartProcess(UnrealEnginePath, false);
                Logger.Info("Unreal Engine process started!");
                unrealEngineModel = new UnrealEngineModel();
                Logger.Info("Unreal Engine is ready to use!");
            });
        }

        public async Task StopUnrealEngine()
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

        public async Task StartSensors()
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

        public async Task StopSensors()
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

        public async Task StartScreenCapture()
        {
            await Task.Run(() =>
            {
                // TODO implementation
                Logger.Warn("Screen Capture currently not supported!");
            });
        }

        public async void AutoStressTest()
        {
            await StartUnrealEngine();
            await StartScreenCapture();
            await StartSensors();
        }

    }
}
