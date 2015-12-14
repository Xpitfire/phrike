using Phrike.GroundControl.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.ViewModels
{

    public static class GCColors
    {
        public static Brush Active = Brushes.GreenYellow;
        public static Brush Disabled = Brushes.OrangeRed;
    }

    public class StressTestViewModel : INotifyPropertyChanged
    {
        private static StressTestViewModel instance = null;
        public static StressTestViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StressTestViewModel();
                }
                return instance;
            }
            private set
            {
                if (instance != value)
                {
                    instance = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Brush unrealStatusColor;
        private Brush sensorStatusColor;
        private Brush screenCapturingStatusColor;

        public StressTestViewModel()
        {
            SetAllStatusToDisabled();
            Instance = this;
        }

        public Brush UnrealStatusColor
        {
            get
            {
                return unrealStatusColor;
            }
            set
            {
                if (unrealStatusColor != value)
                {
                    unrealStatusColor = value;
                    OnPropertyChanged();
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
                if (sensorStatusColor != value)
                {
                    sensorStatusColor = value;
                    OnPropertyChanged();
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
                if (screenCapturingStatusColor != value)
                {
                    screenCapturingStatusColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetAllStatusToActive()
        {
            UnrealStatusColor = GCColors.Active;
            SensorStatusColor = GCColors.Active;
            ScreenCapturingStatusColor = GCColors.Active;
        }

        public void SetAllStatusToDisabled()
        {
            UnrealStatusColor = GCColors.Disabled;
            SensorStatusColor = GCColors.Disabled;
            ScreenCapturingStatusColor = GCColors.Disabled;
        }

        #region PropertyChanged Handler

        /// <summary>
        /// Handle the Property change Binding updates.
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
