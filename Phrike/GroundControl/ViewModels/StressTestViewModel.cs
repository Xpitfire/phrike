using System.ComponentModel;
using System.Runtime.CompilerServices;
using Phrike.GroundControl.Model;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using NLog;
using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{
    /// <summary>
    /// The StressTest.xaml ViewModel to control the hardware components and
    /// run multiple sub-process calls and Tasks.
    /// </summary>
    class StressTestViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private SensorsModel sensorsModel;
        private UnrealEngineModel unrealEngineModel;

        #region Status Info Properties
        private Brush unrealStatusColor;
        private Brush sensorStatusColor;
        private Brush screenCapturingStatusColor;

        public Brush UnrealStatusColor
        {
            get { return unrealStatusColor; }
            set
            {
                unrealStatusColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("UnrealStatusColor"));
                } 
            }
        }

        public Brush SensorStatusColor
        {
            get
            {
                return sensorStatusColor;
            }
            set
            {
                sensorStatusColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SensorStatusColor"));
                } 
            }
        }

        public Brush ScreenCapturingStatusColor
        {
            get
            {
                return screenCapturingStatusColor;
            }
            set
            {
                screenCapturingStatusColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenCapturingStatusColor"));
                } 
            }
        }

        public Brush Activate = Brushes.GreenYellow;
        public Brush Disable = Brushes.OrangeRed;

        private void InitStatusColors()
        {
            UnrealStatusColor = Disable;
            SensorStatusColor = Disable;
            ScreenCapturingStatusColor = Disable;
        }

        #endregion

        public StressTestViewModel()
        {
            Instance = this;
            InitStatusColors();
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
                ProcessModel.StartProcess(UnrealEngineModel.UnrealEnginePath, false);
                Logger.Info("Unreal Engine process started!");
                // create the Unreal Engine communication object
                unrealEngineModel = new UnrealEngineModel();
                Logger.Info("Unreal Engine is ready to use!");
                UnrealStatusColor = Activate;
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
                ProcessModel.StopProcess(UnrealEngineModel.UnrealEnginePath);
                Logger.Info("Unreal Engine process stoped!");
                ScreenCapturingStatusColor = Disable;
                UnrealStatusColor = Disable;
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

                var active = sensorsModel.StartRecording();
                if (!active)
                {
                    sensorsModel = null;
                }
                else
                {
                    SensorStatusColor = Activate;
                }
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
                SensorStatusColor = Disable;
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
                ScreenCapturingStatusColor = Activate;
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
                ScreenCapturingStatusColor = Disable;
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


        #region PropertyChanged Handling

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handle the Property change Binding updates.
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
