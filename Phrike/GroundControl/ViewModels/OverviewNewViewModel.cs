using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Controller;
using Phrike.GroundControl.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Phrike.GroundControl.ViewModels
{
    public class OverviewNewViewModel : INotifyPropertyChanged
    {
        public static OverviewNewViewModel Instance;

        private ScenarioVM currentScenario;
        private SubjectVM currentSubject;

        public event PropertyChangedEventHandler PropertyChanged;

        public OverviewNewViewModel()
        {
            Instance = this;
            StartStressTestCommand = new RelayCommand(StartStressTest);
            //SelectSubjectCommand = new RelayCommand(SelectSubject);
            //SelectScenarioCommand = new RelayCommand(SelectScenario);
        }

        public ScenarioVM CurrentScenario
        {
            get { return currentScenario; }
            set
            {
                if (currentScenario != value)
                {
                    currentScenario = value;
                    OnPropertyChanged();
                }
            }
        }

        public SubjectVM CurrentSubject
        {
            get
            {
                return currentSubject;
            }
            set
            {
                if (currentSubject != value)
                {
                    currentSubject = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand StartStressTestCommand { get; private set; }

        private void StartStressTest(object parameter)
        {
            StressTestController hc = new StressTestController();

            hc.StartStressTest(CurrentSubject, CurrentScenario);

            Console.WriteLine("Test started...");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
