using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

using DataAccess;
using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    //public class SurveyCollectionVM : INotifyPropertyChanged
    //{
    //    private SurveyVM currentSurvey;
    //    public ObservableCollection<SurveyVM> Surveys { get; set; }
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public SurveyCollectionVM()
    //    {
    //        Surveys = new ObservableCollection<SurveyVM>();
    //        currentSurvey = new SurveyVM();
    //        if (DataLoadHelper.IsLoadDataActive())
    //            LoadSurveys();
    //    }

    //    public SurveyVM CurrentSurvey
    //    {
    //        get { return this.currentSurvey; }
    //        set
    //        {
    //            if (currentSurvey != value)
    //            {
    //                this.currentSurvey = value;
    //                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSurvey)));
    //            }
    //        }
    //    }

    //    private async void LoadSurveys()
    //    {
    //        Surveys.Clear();

    //        using (var x = new UnitOfWork())
    //        {
    //            IEnumerator<Survey> enu = x.SurveyRepository.Get().GetEnumerator();
    //            while (await Task.Factory.StartNew(() => enu.MoveNext()))
    //            {
    //                Surveys.Add(new SurveyVM(enu.Current));
    //            }
    //        }
    //    }

    //}

    //public class SurveyAnswerVM : INotifyPropertyChanged
    //{
    //    private SurveyAnswer answer;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public SurveyAnswerVM(SurveyAnswer answer)
    //    {
    //        this.answer = answer;
    //    }

    //    public string Name
    //    {
    //        get { return this.answer.ToString(); }
    //        set
    //        {
    //            switch (value)
    //            {
    //                case nameof(SurveyAnswer.Perfect):
    //                    answer = SurveyAnswer.Perfect;
    //                    break;
    //                case nameof(SurveyAnswer.Good):
    //                    this.answer = SurveyAnswer.Good;
    //                    break;
    //                case nameof(SurveyAnswer.Gratifying):
    //                    this.answer = SurveyAnswer.Gratifying;
    //                    break;
    //                case nameof(SurveyAnswer.Bad):
    //                    this.answer = SurveyAnswer.Bad;
    //                    break;
    //                case nameof(SurveyAnswer.Worst):
    //                    this.answer = SurveyAnswer.Worst;
    //                    break;
    //            }
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
    //        }
    //    }
    //}

    public class SurveyCollectionVM : INotifyPropertyChanged
    {
        private SurveyVM currentSurvey;
        public ObservableCollection<SurveyVM> Surveys { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyCollectionVM()
        {
            Surveys = new ObservableCollection<SurveyVM>();
            currentSurvey = new SurveyVM();
            if (DataLoadHelper.IsLoadDataActive())
                LoadSurveys();
        }

        public SurveyVM CurrentSurvey
        {
            get { return this.currentSurvey; }
            set
            {
                if (currentSurvey != value)
                {
                    this.currentSurvey = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSurvey)));
                }
            }
        }

        private async void LoadSurveys()
        {
            Surveys.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Survey> enu = x.SurveyRepository.Get().GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Surveys.Add(new SurveyVM(enu.Current));
                }
            }
        }

    }

    public class SurveyVM : INotifyPropertyChanged
    {
        private Survey survey;
        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyVM(Survey survey)
        {
            this.survey = survey;
        }

        public SurveyVM()
        {
            survey = new Survey();
        }

        public int Id
        {
            get { return survey != null ? survey.Id : 0; }
            set
            {
                if (survey.Id != value)
                {
                    survey.Id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
                }
            }
        }

        public string Name
        {
            get { return survey != null ? survey.Name : ""; }
            set
            {
                if (survey.Name != value)
                {
                    survey.Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public string Description
        {
            get { return survey != null ? survey.Description : ""; }
            set
            {
                if (survey.Description != value)
                {
                    survey.Description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }
    }

    public class SurveyQuestionCollectionVM : INotifyPropertyChanged
    {
        private SurveyQuestionVM currentSurveyQuestion;
        public ObservableCollection<SurveyQuestionVM> SurveyQuestions { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyQuestionCollectionVM()
        {
            SurveyQuestions = new ObservableCollection<SurveyQuestionVM>();
            currentSurveyQuestion = new SurveyQuestionVM();
            if (DataLoadHelper.IsLoadDataActive())
                this.LoadSurveyQuestions();
        }

        public SurveyQuestionVM CurrentSurveyQuestion
        {
            get { return this.currentSurveyQuestion; }
            set
            {
                if (currentSurveyQuestion != value)
                {
                    this.currentSurveyQuestion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSurveyQuestion)));
                }
            }
        }

        public async void LoadSurveyQuestions(int id = 0)
        {
            SurveyQuestions.Clear();
            IEnumerator<SurveyQuestion> enu;
            using (var x = new UnitOfWork())
            {
                enu = id > 0
                    ? x.SurveyQuestionRepository.Get(question => question.Id == id).GetEnumerator()
                    : x.SurveyQuestionRepository.Get().GetEnumerator();

                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    SurveyQuestions.Add(new SurveyQuestionVM(enu.Current));
                }
            }
        }
    }

    public class SurveyQuestionVM : INotifyPropertyChanged
    {
        private SurveyQuestion surveyQuestion;

        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyQuestionVM(SurveyQuestion surveyQuestion)
        {
            this.surveyQuestion = surveyQuestion;
        }

        public SurveyQuestionVM()
        {
            surveyQuestion = new SurveyQuestion();
        }

        public string Question
        {
            get { return surveyQuestion.Question; }
            set
            {
                if (surveyQuestion.Question != value)
                {
                    surveyQuestion.Question = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Question)));
                }
            }
        }

        public int SurveyId
        {
            get { return surveyQuestion.Survey.Id; }
            set
            {
                if (surveyQuestion.Survey.Id != value)
                {
                    surveyQuestion.Survey.Id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SurveyId)));
                }
            }
        }

        public override string ToString()
        {
            return Question;
        }
    }

    class SurveyResultCollectionVM : INotifyPropertyChanged
    {
        private SurveyResultVM currentSurveyResult;
        public ObservableCollection<SurveyResultVM> SurveyResults { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyResultCollectionVM()
        {
            SurveyResults = new ObservableCollection<SurveyResultVM>();
            currentSurveyResult = new SurveyResultVM();
            if (DataLoadHelper.IsLoadDataActive())
                this.LoadSurveyResults();
        }

        public SurveyResultVM CurrentSurveyResult
        {
            get { return this.currentSurveyResult; }
            set
            {
                if (currentSurveyResult != value)
                {
                    this.currentSurveyResult = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSurveyResult)));
                }
            }
        }

        private async void LoadSurveyResults()
        {
            SurveyResults.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<SurveyResult> enu = x.SurveyResultRepository.Get().GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    SurveyResults.Add(new SurveyResultVM(enu.Current));
                }
            }
        }

    }

    class SurveyResultVM : INotifyPropertyChanged
    {
        private SurveyResult surveyResult;

        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyResultVM(SurveyResult surveyResult)
        {
            this.surveyResult = surveyResult;
        }

        public SurveyResultVM()
        {
            this.surveyResult = new SurveyResult();
        }

        public SurveyAnswer Answer
        {
            get { return this.surveyResult.Answer; }
            set
            {
                if (this.surveyResult.Answer != value)
                {
                    this.surveyResult.Answer = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Answer)));
                }
            }
        }

        public int SurveyQuestionId
        {
            get { return this.surveyResult.SurveyQuestion.Id; }
            set
            {
                if (this.surveyResult.SurveyQuestion.Id != value)
                {
                    this.surveyResult.SurveyQuestion.Id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SurveyQuestionId)));
                }
            }
        }
    }

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

        public string FullTitle => $"{test.Time:f} | {test.Title} ({test.Scenario.Name}) | {test.Subject?.FirstName} {test.Subject?.LastName}";

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

        public DateTime Date
        {
            get { return test.Time; }
            set
            {
                if (test.Time != value)
                {
                    test.Time = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
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

        public bool UseDefaultIcon { get { return string.IsNullOrEmpty(AvatarPath); } }

        public string ImagePath
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

        public string FullName
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
        public IEnumerable<string> AvailableCountries => (new List<string>() { "AT", "DE", "CH" });
        public IEnumerable<RhFactor> AvailableRhFactors => (RhFactor[])Enum.GetValues(typeof(RhFactor));
        public IEnumerable<BloodType> AvailableBloodTypes => (BloodType[])Enum.GetValues(typeof(BloodType));
        public IEnumerable<string> AvailableServiceRanks => (new List<string>() { "Rekrut", "Gefreiter", "Korporal", "Zugsführer", "Wachtmeister", "Oberwachtmeister", "Stabswachtmeister", "Oberstabswachtmeister", "Offiziersstellvertreter", "Vizeleutnant", "Fähnrich", "Leutnant", "Oberleutnant", "Hauptmann", "Major", "Oberstleutnant", "Oberst", "Brigardier", "Generalmajor", "Generalleutnant", "General" });
        #region Property Propagation

        public int Id
        {
            get { return subject.Id; }
        }

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
        public string FirstName
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
        public string AvatarPath
        {
            get
            {
                //return string.IsNullOrEmpty(subject.AvatarPath) ? "" : System.IO.Path.Combine(PathHelper.PhrikePicture, subject.AvatarPath);
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

        public string Icon
        {
            get
            {
                if (scenario.ThumbnailPath == null || scenario.ThumbnailPath == string.Empty)
                {
                    return DefaultDataProvider.PrepareDefaultScenarioIcon();
                }
                else
                {
                    return System.IO.Path.Combine(PathHelper.PhrikePicture, scenario.ThumbnailPath);
                }
            }
        }

        public int Id { get { return scenario.Id; } }

        public String Name { get { return scenario.Name; } }

        public string Description { get { return scenario.Description; } }
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
