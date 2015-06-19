using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {

        public static SettingsViewModel Instance { get; private set; }

        public string SensorComPort { get; set; }

        public SettingsViewModel()
        {
            Instance = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
