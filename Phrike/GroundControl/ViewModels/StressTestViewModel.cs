using Phrike.GroundControl.Model;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Phrike.GroundControl.ViewModels
{
    /// <summary>
    /// The StressTest.xaml ViewModel to control the hardware components and
    /// run multiple sub-process calls and Tasks.
    /// </summary>
    class StressTestViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Execution path of the Unreal Engine.
        /// </summary>
        public const string UnrealEnginePath = @"C:\public\OperationPhrike\Phrike\GroundControl\UnrealData\Balance.exe";

        private SensorsModel sensorsModel;
        private UnrealEngineModel unrealEngineModel;

        public StressTestViewModel()
        {
            Instance = this;
        }

        public static StressTestViewModel Instance { get; private set; }

        // Methods for UI button click bindings.
        #region Binding Methods

        public async void StartUnrealEngine()
        {
            await StartUnrealEngineTask();
        }
        public async void StopUnrealEngine()
        {
            await StopUnrealEngineTask();
        }
        public async void StartSensors()
        {
            await StartSensorsTask();
        }
        public async void StopSensors()
        {
            await StopSensorsTask();
        }
        public async void StartScreenCapture()
        {
            await StartScreenCaptureTask();
        }
        public async void StopScreenCapture()
        {
            await StopScreenCaptureTask();
        }

        /// <summary>
        /// Save close of running Tasks.
        /// </summary>
        public async void ApplicationClose()
        {
            await ApplicationCloseTask();
        }

        /// <summary>
        /// Start a new stress test sequence.
        /// </summary>
        public async void AutoStressTest()
        {
            // show progress animation in UI
            MainViewModel.Instance.ShowProgressMessage("Preparing a new stress test", "All engines are initializing and staring up! Please wait...");
            // start sequencial tasks in a new Thread
            var stressTestThread = new Thread(async () =>
            {
                await StartUnrealEngineTask();
                await StartScreenCaptureTask();
                await StartSensorsTask();
            });
            // start the Thread and stop the animation with a delay
            await Task.Run(() =>
            {
                stressTestThread.Start();
                Thread.Sleep(5000);
            });
            // stop progress animation
            MainViewModel.Instance.CloseProgressMessage();
        }

        #endregion

        // Async Task methods for user interaction.
        #region Async Tasks

        /// <summary>
        /// Create an instance of the Unreal Engine.
        /// </summary>
        /// <returns></returns>
        public async Task StartUnrealEngineTask()
        {
            await Task.Run(() =>
            {
                // start the external application sub-process
                ProcessModel.StartProcess(UnrealEnginePath, false);
                Logger.Info("Unreal Engine process started!");
                // create the Unreal Engine communication object
                unrealEngineModel = new UnrealEngineModel();
                Logger.Info("Unreal Engine is ready to use!");
            });
        }
        /// <summary>
        /// Close the Unreal Engine instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Starting sensors recording instance.
        /// </summary>
        /// <returns></returns>
        public async Task StartSensorsTask()
        {
            await Task.Run(() =>
            {
                if (sensorsModel != null)
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
        /// <summary>
        /// Stopping sensors recording instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Starting full screen capturing instance.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Stopping full screen capturing instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Closing all running application tasks.
        /// </summary>
        /// <returns></returns>
        public async Task ApplicationCloseTask()
        {
            await StopSensorsTask();
            await StopScreenCaptureTask();
            await StopUnrealEngineTask();
        }

        #endregion

        /// <summary>
        /// Show error message to user.
        /// </summary>
        /// <param name="message"></param>
        private void ShowStressTestError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Stress Test Error", message);
        }
    }
}
