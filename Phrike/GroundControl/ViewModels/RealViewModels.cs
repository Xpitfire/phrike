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
            get { return subject.AvatarPath == null || subject.AvatarPath != String.Empty ? subject.AvatarPath : System.IO.Path.Combine( PathHelper.PhrikePicture, "user.png"); }
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
        private Scenario currentScenario;
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

        public Scenario CurrentScenario
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
            get {
                return scenario.ThumbnailPath == null || scenario.ThumbnailPath == String.Empty
                           ? System.IO.Path.Combine(PathHelper.PhrikePicture, "scenario.png")
                           : scenario.MinimapPath;
            }
        }

        public String Name { get { return scenario.Name; } }

        public String Description { get { return scenario.Description; } }
    }

    class OverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ScenarioVM CurrentScenario { get; set; }
        public Subject CurrentSubject { get; set; }
    }
}
