using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();

            for (var i = 1; i <= 12; ++i)
            {
                var comPort = "COM" + i;
                ComboBoxItem cboxItem = new ComPortComboBoxItem
                {
                    Content = comPort,
                    ComPort = comPort + ":" // the colon is required for the sensor interface
                };
                SensorComPortComboBox.Items.Add(cboxItem);
            }
            SensorComPortComboBox.SelectedIndex = 6;
        }

        /// <summary>
        /// Sensor COM Port combo box item with overridden ToString method
        /// for correct Binding representation.
        /// </summary>
        public class ComPortComboBoxItem : ComboBoxItem
        {
            public string ComPort { get; set; }

            public override string ToString()
            {
                return ComPort;
            }
        }
    }
}
