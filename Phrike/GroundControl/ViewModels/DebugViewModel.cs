using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using DataAccess;

using DataModel;

using Microsoft.Win32;

using NLog;
using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Controller;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    /// <summary>
    /// The StressTest.xaml ViewModel to control the hardware components and
    /// run multiple sub-process calls and Tasks.
    /// </summary>
    public class DebugViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private SensorsController sensorsModel;
        private UnrealEngineController unrealEngineModel;
        private ICommand addSensorCmd;

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

        public DebugViewModel()
        {
            Instance = this;
            InitStatusColors();
        }

        public static DebugViewModel Instance { get; private set; }

        public ICommand AddSensorFile
            => addSensorCmd ?? (addSensorCmd = new RelayCommand(AddSensorFileImpl));

        private void AddSensorFileImpl(object obj)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "GMobiLabPlus (*.bin)|*.bin|Biofeedback 2000 (*.csv)|*.csv"
            };
            if (dlg.ShowDialog() != true)
                return;

            SensorAuxDataHelper.ImportSensorDataFile(dlg.FileName, testId: 1);
        }

        // Methods for UI button click bindings.
        #region Binding Methods

        /// <summary>
        /// Save close of running Tasks.
        /// </summary>
        public async void ApplicationClose()
        {
            await ApplicationCloseTask();
        }

        #endregion

        // Async Task methods for user interaction.
        #region Async Tasks
 
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
                    return;
                }

                unrealEngineModel.Close();
                unrealEngineModel = null;
                ProcessController.StopProcess(UnrealEngineController.UnrealEnginePath);
                Logger.Info("Unreal Engine process stoped!");
                ScreenCapturingStatusColor = Disable;
                UnrealStatusColor = Disable;
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
                    return;
                }
                sensorsModel.Close();
                sensorsModel = null;
                Logger.Info("Sensors recording successfully stopped!");
                SensorStatusColor = Disable;
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
                    DialogHelper.ShowErrorDialog("Videoaufnahme konnte nicht beendet werden.");
                    return;
                }

                // TODO: Screencapturing nicht mehr in Unreal Engine
                //unrealEngineModel.StopCapture();
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
