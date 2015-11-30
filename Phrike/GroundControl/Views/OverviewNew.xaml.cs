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
using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for OverviewNew.xaml
    /// </summary>
    public partial class OverviewNew : UserControl
    {
        public OverviewNew()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                this.DataContext = new OverviewVM();
            };
        }

        private void SelectNewSubject(object sender, RoutedEventArgs e)
        {
            // TODO: call UserSelect uc
        }

        private void SelectNewScenario(object sender, RoutedEventArgs e)
        {
            // TODO: call ScenarioSelect uc        
        }
    }
}
