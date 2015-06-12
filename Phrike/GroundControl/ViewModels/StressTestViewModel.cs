using Phrike.GroundControl.Model;
using System.Threading;
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

        public StressTestViewModel()
        {
            Instance = this;
        }

        public static StressTestViewModel Instance { get; private set; }

        #region Binding Methods

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

        public delegate void UpdateProgressFunc(double value);

        public static void thirdPartyApplicationInstallWorkflow(App app, UpdateProgressFunc UpdateProgress)
        {
            UpdateProgress(100);
        }


        public async void AutoStressTest()
        {
            MainViewModel.Instance.ShowProgressMessage("Preparing a new stress test", "All engines are initializing and staring up! Please wait...");
            var stressTestThread = new Thread(new ThreadStart(async () =>
            {
                await StartUnrealEngineTask();
                await StartScreenCaptureTask();
                await StartSensorsTask();
            }));
            await Task.Run(() =>
            {
                Thread.Sleep(5000);
                stressTestThread.Start();
            });
            MainViewModel.Instance.CloseProgressMessage();
        }

        #endregion

        #region Async Tasks

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
                    const string message = "Could not stop the Unreal Engine! No Unreal Engine instance active.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
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
                if (sensorsModel == null)
                {
                    const string message = "Could not start sensors recording! Recording task is already running.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

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
                    const string message = "Could not stop sensors recording! No sensors recording instance enabled.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }
                sensorsModel.Close();
                sensorsModel = null;
                Logger.Info("Sensors recording successfully stopped!");
            });
        }

        public async Task StartScreenCaptureTask()
        {
            await Task.Run(() =>
            {
                if (unrealEngineModel == null)
                {
                    const string message = "Could not start screen recording! Recording task is already running.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

                unrealEngineModel.StartCapture();
                Logger.Info("Screen Capture successfully started!");
            });
        }
        public async Task StopScreenCaptureTask()
        {
            await Task.Run(() =>
            {
                if (unrealEngineModel == null)
                {
                    const string message = "Could not stop screen recording! No recording running.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

                unrealEngineModel.StopCapture();
                Logger.Info("Screen Capture successfully stopped!");
            });
        }

        public async Task ApplicationCloseTask()
        {
            await StopSensorsTask();
            await StopScreenCaptureTask();
            await StopUnrealEngineTask();
        }

        #endregion


        private void ShowStressTestError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Stress Test Error", message);
        }
    }
}
