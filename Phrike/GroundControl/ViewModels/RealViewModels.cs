using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DataAccess;
using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    class TestCollectionVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TestVM> Tests { get; set; }
        private TestVM currentTest;

        public TestCollectionVM()
        {
            Tests = new ObservableCollection<TestVM>();
            currentTest = new TestVM();
            if (DataLoadHelper.IsLoadDataActive())
                LoadTest();
        }

        private async void LoadTest()
        {
            Tests.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Test> enu = x.TestRepository.Get(includeProperties: "Scenario,Subject").GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Tests.Add(new TestVM(enu.Current));
                }
            }
        }

        public TestVM CurrentTest
        {
            get { return this.currentTest; }
            set
            {
                if (currentTest != value)
                {
                    this.currentTest = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTest)));
                }
            }
        }
    }

    internal class TestVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Test test;
        
        public Test Test
        {
            get { return test; }
            set
            {
                test = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Test)));
            }
        }

        public TestVM(Test test)
        {
            this.test = test;
        }

        public TestVM()
        {
            test = new Test();
        }

        public string FullTitle => $"{test.Title} {test.Subject?.LastName} {test.Scenario?.Name}";
        
        public string Title
        {
            get { return test.Title; }
            set
            {
                if (test.Title != value)
                {
                    test.Title = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
                }
            }
        }

        public Scenario Scenario
        {
            get { return test.Scenario; }
            set
            {
                if (test.Scenario != value)
                {
                    test.Scenario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Scenario)));
                }
            }
        }

        public Subject Subject
        {
            get { return test.Subject; }
            set
            {
                if (test.Subject != value)
                {
                    test.Subject = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Subject)));
                }
            }
        }

    }

    class SubjectCollectionVM : INotifyPropertyChanged
    {
        private SubjectVM currentSubject;
        public ObservableCollection<SubjectVM> Subjects { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SubjectCollectionVM()
        {
            Subjects = new ObservableCollection<SubjectVM>();
            currentSubject = new SubjectVM();
            if (DataLoadHelper.IsLoadDataActive())
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


    public class SubjectVM : INotifyPropertyChanged
    {
        private Subject subject;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool insertsDone = true;

        private string oldAvatarPath;

        public SubjectVM(Subject subject)
        {
            this.subject = subject;
            oldAvatarPath = subject.AvatarPath;
        }

        public SubjectVM()
        {
            subject = new Subject();
        }

        internal bool Submit(out string message)
        {
            using (var x = new UnitOfWork())
            {
                try
                {
                    if (subject.Id == default(int))
                    {
                        string path = subject.AvatarPath;
                        subject.AvatarPath = oldAvatarPath;
                        x.SubjectRepository.Insert(subject);
                        x.Save();

                        FileStorageHelper.SetSubjectAvatar(path, subject, x);
                        oldAvatarPath = path;
                    }
                    else
                    {
                        if (AvatarPathChanged)
                        {
                            string path = subject.AvatarPath;
                            subject.AvatarPath = oldAvatarPath;
                            x.SubjectRepository.Update(subject);
                            x.Save();

                            FileStorageHelper.SetSubjectAvatar(path, subject, x);
                            oldAvatarPath = path;
                        }
                        else
                        {
                            x.SubjectRepository.Update(subject);
                            x.Save();
                        }
                    }
                    InsertsDone = true;
                    message = "";
                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    message = "Fehler beim erstellen eines neuen Benutzers:\n";
                    foreach (var error in ex.EntityValidationErrors.FirstOrDefault()?.ValidationErrors)
                    {
                        message += $"{error.ErrorMessage}\n";
                    }
                    return false;
                }
                catch (Exception end)
                {
                    message = "Unbekannter Fehler:\n";
                    message += end.Message;
                    return false;
                }
            }
        }

        internal void Cancel()
        {
            InsertsDone = true;
        }

        internal void Flush()
        {
            LastName = "";
            FirstName = "";
            DateOfBirth = DateTime.MinValue;
            Gender = default(Gender);
            CountryCode = "";
            City = "";
            PostalCode = "";
            Street = "";
            ServiceRank = "";
            Function = "";
            Conditions = "";
            BloodType = default(BloodType);
            RhFactor = default(RhFactor);
            AvatarPath = "";
        }

        public bool UseDefaultIcon { get { return String.IsNullOrEmpty(AvatarPath); } }

        public String ImagePath
        {
            get
            {
                if (UseDefaultIcon)
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

        public bool InsertsDone
        {
            get { return insertsDone; }
            set
            {
                if (insertsDone != value)
                {
                    insertsDone = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InsertsDone)));
                }
            }
        }

        private bool AvatarPathChanged { get; set; }

        public IEnumerable<Gender> AvailableGenders => (Gender[])Enum.GetValues(typeof(Gender));
        public IEnumerable<String> AvailableCountries => (new List<string>() { "AT", "DE", "CH" });
        public IEnumerable<RhFactor> AvailableRhFactors => (RhFactor[])Enum.GetValues(typeof(RhFactor));
        public IEnumerable<BloodType> AvailableBloodTypes => (BloodType[])Enum.GetValues(typeof(BloodType));
        public IEnumerable<String> AvailableServiceRanks => (new List<string>() { "Rekrut", "Gefreiter", "Korporal", "Zugsführer", "Wachtmeister", "Oberwachtmeister", "Stabswachtmeister", "Oberstabswachtmeister", "Offiziersstellvertreter", "Vizeleutnant", "Fähnrich", "Leutnant", "Oberleutnant", "Hauptmann", "Major", "Oberstleutnant", "Oberst", "Brigardier", "Generalmajor", "Generalleutnant", "General" });
        #region Property Propagation

        public String LastName
        {
            get { return subject.LastName; }
            set
            {
                if (subject.LastName != value)
                {
                    subject.LastName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }
        public String FirstName
        {
            get { return subject.FirstName; }
            set
            {
                if (subject.FirstName != value)
                {
                    subject.FirstName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }
        public DateTime DateOfBirth
        {
            get { return subject.DateOfBirth; }
            set
            {
                if (value == DateTime.MinValue)
                {
                    subject.DateOfBirth = subject.DateOfBirth = DateTime.Today.Subtract(new TimeSpan(18 * 365, 0, 0, 0));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
                }
                else if (subject.DateOfBirth != value)
                {
                    subject.DateOfBirth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
                }
            }
        }
        public Gender Gender
        {
            get { return subject.Gender; }
            set
            {
                if (subject.Gender != value)
                {
                    subject.Gender = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Gender)));
                }
            }
        }
        public string CountryCode
        {
            get { return subject.CountryCode; }
            set
            {
                if (subject.CountryCode != value)
                {
                    subject.CountryCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountryCode)));
                }
            }
        }
        public string City
        {
            get { return subject.City; }
            set
            {
                if (subject.City != value)
                {
                    subject.City = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
                }
            }
        }
        public string PostalCode
        {
            get { return subject.PostalCode; }
            set
            {
                if (subject.PostalCode != value)
                {
                    subject.PostalCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PostalCode)));
                }
            }
        }
        public string Street
        {
            get { return subject.Street; }
            set
            {
                if (subject.Street != value)
                {
                    subject.Street = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Street)));
                }
            }
        }
        public string ServiceRank
        {
            get { return subject.ServiceRank; }
            set
            {
                if (subject.ServiceRank != value)
                {
                    subject.ServiceRank = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServiceRank)));
                }
            }
        }
        public string Function
        {
            get { return subject.Function; }
            set
            {
                if (subject.Function != value)
                {
                    subject.Function = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Function)));
                }
            }
        }
        public string Conditions
        {
            get { return subject.Conditions; }
            set
            {
                if (subject.Conditions != value)
                {
                    subject.Conditions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Conditions)));
                }
            }
        }
        public BloodType BloodType
        {
            get { return subject.BloodType; }
            set
            {
                if (subject.BloodType != value)
                {
                    subject.BloodType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BloodType)));
                }
            }
        }
        public RhFactor RhFactor
        {
            get { return subject.RhFactor; }
            set
            {
                if (subject.RhFactor != value)
                {
                    subject.RhFactor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RhFactor)));
                }
            }
        }
        public String AvatarPath
        {
            get
            {
                //return String.IsNullOrEmpty(subject.AvatarPath) ? "" : System.IO.Path.Combine(PathHelper.PhrikePicture, subject.AvatarPath);
                return subject.AvatarPath;
            }
            set
            {
                if (subject.AvatarPath != value)
                {
                    subject.AvatarPath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvatarPath)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
                    AvatarPathChanged = true;
                }
            }
        }

        #endregion
    }

    public class ScenarioCollectionVM : INotifyPropertyChanged
    {
        private ScenarioVM currentScenario;
        public ObservableCollection<ScenarioVM> Scenarios { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ScenarioCollectionVM()
        {
            this.Scenarios = new ObservableCollection<ScenarioVM>();
            if (DataLoadHelper.IsLoadDataActive())
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

    public class ScenarioVM : INotifyPropertyChanged
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

    /*
    class OverviewVM : INotifyPropertyChanged
    {
        private ScenarioVM currentScenario;
        private SubjectVM currentSubject;

        public event PropertyChangedEventHandler PropertyChanged;


        public ScenarioVM CurrentScenario
        {
            get { return currentScenario; }
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
    */
}
