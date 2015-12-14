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
    public partial class NewStressTest : UserControl
    {
        private enum ViewState
        {
            Home,
            Subject,
            Scenario
        };

        private static ViewState state = ViewState.Home;
        private NewStressTestViewModel overviewNewViewModel;

        public NewStressTest()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                this.DataContext = overviewNewViewModel = new NewStressTestViewModel();
            };

            // todo: routed event from select-uc's
            // http://stackoverflow.com/questions/3067617/raising-an-event-on-parent-window-from-a-user-control-in-net-c-sharp
        }

        private void SwitchToSubjectSelectionView(object sender, RoutedEventArgs e)
        {
            wpButtons.Visibility = Visibility.Hidden;
            ucUser.Visibility = Visibility.Visible;
            btnBack.IsEnabled = true;
            state = ViewState.Subject;
        }

        private void SwitchToScenarioSelectionView(object sender, RoutedEventArgs e)
        {
            wpButtons.Visibility = Visibility.Hidden;
            ucScenario.Visibility = Visibility.Visible;
            btnBack.IsEnabled = true;
            state = ViewState.Scenario;
        }

        private void GoBack()
        {
            switch (state)
            {
                case ViewState.Home:
                    break;
                case ViewState.Scenario:
                    ucScenario.Visibility = Visibility.Hidden;
                    wpButtons.Visibility = Visibility.Visible;
                    state = ViewState.Home;
                    btnBack.IsEnabled = false;
                    break;
                case ViewState.Subject:
                    ucUser.Visibility = Visibility.Hidden;
                    wpButtons.Visibility = Visibility.Visible;
                    state = ViewState.Home;
                    btnBack.IsEnabled = false;
                    break;
            }
        }

        private void BtnBack_OnClick(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void SelectSubject(object sender, RoutedEventArgs e)
        {
            var subjectCollectionVM = (SubjectCollectionVM)((UserSelect)sender).DataContext;
            overviewNewViewModel.CurrentSubject = subjectCollectionVM.CurrentSubject;
            GoBack();
            //MessageBox.Show(curr.FullName+ " selected");
        }

        private void SelectScenario(object sender, RoutedEventArgs e)
        {
            var scenarioCollectionVM = (ScenarioCollectionVM)((ScenarioSelect)sender).DataContext;
            overviewNewViewModel.CurrentScenario = scenarioCollectionVM.CurrentScenario;
            GoBack();
            //MessageBox.Show(curr.Name + " selected");
        }
    }
}
