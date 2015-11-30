using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using OxyPlot;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    class SubjectCollectionVM : INotifyPropertyChanged
    {
        private SubjectVM currentSubject;
        public ObservableCollection<SubjectVM> Subjects { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SubjectCollectionVM()
        {
            Subjects = new ObservableCollection<SubjectVM>();

            LoadSubjects();
        }

        public SubjectVM CurrentSubject
        {
            get { return this.currentSubject; }
            set
            {
                if (currentSubject != value)
                {
                    this.currentSubject = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSubject)));
                }
            }
        }

        public async void LoadSubjects()
        {
            Subjects.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Subject> enu = x.SubjectRepository.Get().GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Subjects.Add(new SubjectVM(enu.Current));
                }
            }
        }
    }


    class SubjectVM : INotifyPropertyChanged
    {
        private Subject subject;
        public event PropertyChangedEventHandler PropertyChanged;

        public SubjectVM(Subject subject)
        {
            this.subject = subject;
        }

        public String ImagePath
        {
            get
            {
                if (subject.AvatarPath == null || subject.AvatarPath == String.Empty)
                {
                    return DefaultDataProvider.PrepareDefaultSubjectIcon();
                }
                else
                {
                    return System.IO.Path.Combine(PathHelper.PhrikePicture, subject.AvatarPath);
                }
            }
        }

        public String FullName
        {
            get { return $"{subject.ServiceRank} {subject.FirstName} {subject.LastName}"; }
        }

        public String LastName { get { return subject.LastName; } }
        public String FirstName { get { return subject.FirstName; } }
    }

    class ScenarioCollectionVM : INotifyPropertyChanged
    {
        private ScenarioVM currentScenario;
        public ObservableCollection<ScenarioVM> Scenarios { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ScenarioCollectionVM()
        {
            this.Scenarios = new ObservableCollection<ScenarioVM>();

            LoadScenarios();
        }

        private async void LoadScenarios()
        {
            Scenarios.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Scenario> enu = x.ScenarioRepository.Get().GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Scenarios.Add(new ScenarioVM(enu.Current));
                }
            }
        }

        public ScenarioVM CurrentScenario
        {
            get { return this.currentScenario; }
            set
            {
                if (currentScenario != value)
                {
                    this.currentScenario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScenario)));
                }
            }
        }
    }

    class ScenarioVM : INotifyPropertyChanged
    {
        private Scenario scenario;
        public event PropertyChangedEventHandler PropertyChanged;

        public ScenarioVM(Scenario scenario)
        {
            this.scenario = scenario;
        }

        public String Icon
        {
            get
            {
                if (scenario.ThumbnailPath == null || scenario.ThumbnailPath == String.Empty)
                {
                    return DefaultDataProvider.PrepareDefaultScenarioIcon();
                }
                else
                {
                    return System.IO.Path.Combine(PathHelper.PhrikePicture, scenario.ThumbnailPath);
                }
            }
        }

        public String Name { get { return scenario.Name; } }

        public String Description { get { return scenario.Description; } }
    }

    class OverviewVM : INotifyPropertyChanged
    {
        private ScenarioVM currentScenario;
        private SubjectVM currentSubject;

        public event PropertyChangedEventHandler PropertyChanged;


        public ScenarioVM CurrentScenario
        {
            get { return currentScenario;}
            set
            {
                if (currentScenario != value)
                {
                    currentScenario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScenario)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentSubject)));
                }
            }
        }
    }
}
