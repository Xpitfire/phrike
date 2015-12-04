using System.Windows;

using Phrike.GroundControl.ViewModels;
using Phrike.Sensors;

namespace GdfPlotDemo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new DataBundleViewModel(new DataBundle { DataSeries =
            {
                new DataSeries(new []{1.0, 2.0, 4.0, 0.0}, 2, "src", "ser", Unit.Unknown),
                new DataSeries(new []{0.3, 0.0, 0.2, 0.4}, 2, "src", "ser2", Unit.Unknown)
            }});
        }
    }
}
