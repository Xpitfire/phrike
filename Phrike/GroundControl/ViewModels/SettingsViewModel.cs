using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Models;
using System.Windows.Input;
using System.Configuration;
using System.IO.Ports;

namespace Phrike.GroundControl.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public IEnumerable<Sensor> Sensors { get; private set; }     
        public IEnumerable<string> COMPorts { get; private set; } 

        private int selectedSensorType;

        public static SettingsViewModel Instance { get; private set; }

        public string SensorComPort { get; set; }

        public int SelectedSensorType
        {
            get
            {
                return selectedSensorType;
            }
            set
            {
                if (selectedSensorType != value)
                {
                    selectedSensorType = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsViewModel()
        {
            Instance = this;
            LoadSensors();
            LoadCOMPorts();
        }

        private void LoadSensors()
        {
            Sensors = new List<Sensor>
            {
                new Sensor(0, "g.MOBIlab+"),
                new Sensor(1, "Biofeedback 2000 x-pert")
            };
        }

        private void LoadCOMPorts()
        {
            COMPorts = SerialPort.GetPortNames();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
