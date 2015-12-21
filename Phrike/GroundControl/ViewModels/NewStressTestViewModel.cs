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
    public class NewStressTestViewModel : INotifyPropertyChanged
    {
        public static NewStressTestViewModel Instance;

        private ScenarioVM currentScenario;
        private SubjectVM currentSubject;
        private bool isStartEnabled;
        private bool isStopEnabled;
        private bool isStartVisible;
        private StressTestController stressTestController;

        public event PropertyChangedEventHandler PropertyChanged;

        public NewStressTestViewModel()
        {
            Instance = this;
            stressTestController = new StressTestController();
            StartStressTestCommand = new RelayCommand(StartStressTest);
            StopStressTestCommand = new RelayCommand(StopStressTest);
            IsStartEnabled = false;
            IsStartVisible = true;
            IsStopEnabled = false;
        }

        public ScenarioVM CurrentScenario
        {
            get { return currentScenario; }
            set
            {
                if (currentScenario != value)
                {
                    currentScenario = value;
                    EnableStartButton();
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
                    EnableStartButton();
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStartEnabled
        {
            get
            {
                return isStartEnabled;
            }
            set
            {
                if (isStartEnabled != value)
                {
                    isStartEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStopEnabled
        {
            get
            {
                return isStopEnabled;
            }
            set
            {
                if (isStopEnabled != value)
                {
                    isStopEnabled = value;
                    IsStartVisible = !isStopEnabled;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStartVisible
        {
            get
            {
                return isStartVisible;
            }
            set
            {
                if (isStartVisible != value)
                {
                    isStartVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand StartStressTestCommand { get; private set; }

        private void StartStressTest(object parameter)
        {
            stressTestController.StartStressTest(CurrentSubject, CurrentScenario);
            Console.WriteLine("Test started...");
            IsStartEnabled = false;
            IsStopEnabled = true;
        }

        public ICommand StopStressTestCommand { get; private set; }

        private void StopStressTest(object parameter)
        {
            stressTestController.StopStressTest();
            Console.WriteLine("Test stopped...");
            IsStartEnabled = true;
            IsStopEnabled = false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EnableStartButton()
        {
            IsStartEnabled = currentSubject != null && currentScenario != null;
        }

        public void ApplicationClose()
        {
            stressTestController?.ApplicationCloseTask();
        }
    }
}
