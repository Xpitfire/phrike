using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Controller;
using Phrike.GroundControl.Models;
using System.Windows.Input;
using System.IO.Ports;

namespace Phrike.GroundControl.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ICommand saveSettingsCommand;

        public static SettingsViewModel Instance { get; private set; }

        public static IEnumerable<Sensor> Sensors => SettingsController.Sensors;
        public static IEnumerable<string> COMPorts => SettingsController.COMPorts;

        public SettingsViewModel()
        {
            Instance = this;
            Settings.LoadSettings();
        }

        public string SensorComPort
        {
            get
            {
                return Settings.SensorComPort;
            }
            set
            {
                if (Settings.SensorComPort != value)
                {
                    Settings.SensorComPort = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedSensorType
        {
            get
            {
                return (int)Settings.SelectedSensorType;
            }
            set
            {
                if ((int)Settings.SelectedSensorType != value)
                {
                    Settings.SelectedSensorType = (SensorType)value;
                    OnPropertyChanged();
                }
            }
        }

        public bool RecordingEnabled {
            get
            {
                return Settings.RecordingEnabled;
            }
            set
            {
                if (Settings.RecordingEnabled != value)
                {
                    Settings.RecordingEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SaveProductCommand
        {
            get
            {
                if (saveSettingsCommand == null)
                {
                    saveSettingsCommand = new RelayCommand(
                        param => Settings.SaveSettings()
                    );
                }
                return saveSettingsCommand;
            }
        }
    }
}
