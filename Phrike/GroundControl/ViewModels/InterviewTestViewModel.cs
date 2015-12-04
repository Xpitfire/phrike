// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterviewTestViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The interview test view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using DataModel;

namespace Phrike.GroundControl.ViewModels
{
    /// <summary>
    /// The interview test view model.
    /// </summary>
    public class InterviewTestViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the question list.
        /// </summary>
        public List<string> QuestionList { get; set;  }

        /// <summary>
        /// Gets or sets the result list.
        /// </summary>
        //public List<string> ResultList { get; set; }

        public List<string> SurveyAnsList { get; set; } 

        /// <summary>
        /// Gets or sets the survey name.
        /// </summary>
        public string SurveyName
        {
            get { return Survey.Name; }
            set { SurveyName = value; }
        }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        private static InterviewTestViewModel Instance { get; set; }

        /// <summary>
        /// Gets or sets the survey.
        /// </summary>
        private static Survey Survey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewTestViewModel"/> class.
        /// </summary>
        public InterviewTestViewModel()
        {
            /* test survey */
            Survey = new Survey { Name = "SurveyName", Description = "TestSurveyDescription1" };
            List<SurveyQuestion> sq = new List<SurveyQuestion>();
            Survey.Questions = null;

            SurveyAnsList = new List<string> { "Perfect", "Good", "Gratifying", "Bad", "Worse" };

            SurveyQuestion surveyQuestion1 = new SurveyQuestion { Survey = Survey, Question = "Question1" };
            SurveyQuestion surveyQuestion2 = new SurveyQuestion { Survey = Survey, Question = "Question2" };
            //surveyQuestion1.SurveyResults = null;
            //surveyQuestion2.SurveyResults = null;

            //SurveyResult surveyResult1 = new SurveyResult {SurveyQuestion = surveyQuestion1, Answer = SurveyAnswer.Perfect };
            //SurveyResult surveyResult2 = new SurveyResult {SurveyQuestion = surveyQuestion1, Answer = SurveyAnswer.Bad };

            //var surveyResults1 = new Collection<SurveyResult>();
            //surveyResults1.Add(surveyResult1);
            //surveyResults1.Add(surveyResult2);

            //surveyQuestion1.SurveyResults = surveyResults1;
            //surveyQuestion2.SurveyResults = surveyResults1;

            sq.Add(surveyQuestion1);
            sq.Add(surveyQuestion2);

            Survey.Questions = sq;

            QuestionList = new List<string>();
            foreach (SurveyQuestion q in sq)
            {
                QuestionList.Add(q.Question);
            }

            //ResultList = new List<string>();
            //foreach (SurveyResult r in surveyResults1)
            //{
            //    ResultList.Add(r.Answer.ToString());
            //}

            /* test survey end */

            Instance = this;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}