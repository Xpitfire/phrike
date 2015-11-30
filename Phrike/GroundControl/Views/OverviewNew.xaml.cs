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
        private enum ViewState
        {
            Home,
            Subject,
            Scenario
        };

        private static ViewState state = ViewState.Home;

        public OverviewNew()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                this.DataContext = new OverviewVM();
            };

            // todo: routed event from select-uc's
        }

        private void SelectNewSubject(object sender, RoutedEventArgs e)
        {
            wpButtons.Visibility = Visibility.Hidden;
            ucUser.Visibility = Visibility.Visible;
            btnBack.IsEnabled = true;
            state = ViewState.Subject;
        }

        private void SelectNewScenario(object sender, RoutedEventArgs e)
        {
            wpButtons.Visibility = Visibility.Hidden;
            ucScenario.Visibility = Visibility.Visible;
            btnBack.IsEnabled = true;
            state = ViewState.Scenario;
        }

        private void BtnBack_OnClick(object sender, RoutedEventArgs e)
        {
            switch (state)
            {
                case ViewState.Home:
                    break;
                case ViewState.Scenario:
                    ucScenario.Visibility = Visibility.Hidden;
                    wpButtons.Visibility = Visibility.Visible;
                    state = ViewState.Home;
                    break;
                case ViewState.Subject:
                    ucUser.Visibility = Visibility.Hidden;
                    wpButtons.Visibility = Visibility.Visible;
                    state = ViewState.Home;
                    break;
            }
        }
    }
}
