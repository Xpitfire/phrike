// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterviewTestViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The interview test view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;

using DataAccess;
using DataModel;
using NLog;

namespace Phrike.GroundControl.ViewModels
{
    /// <summary>
    /// The interview test view model.
    /// </summary>
    public class InterviewTestViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the question list.
        /// </summary>
        public List<string> QuestionList { get; set;  }

        /// <summary>
        /// Gets or sets the survey ans list.
        /// </summary>
        public List<string> SurveyAnsList { get; set; } 

        /// <summary>
        /// Gets or sets the survey name.
        /// </summary>
        public string SurveyName { get; set; }

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
            SurveyAnsList = new List<string>
                            {
                                SurveyAnswer.Perfect.ToString(),
                                SurveyAnswer.Good.ToString(),
                                SurveyAnswer.Gratifying.ToString(),
                                SurveyAnswer.Bad.ToString(),
                                SurveyAnswer.Worst.ToString()
                            };

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
              
                Survey testsur = unitOfWork.SurveyRepository.GetByID(1);

                if (testsur?.Questions == null)
                {
                    Console.WriteLine("No questions");
                    return;
                }

                List<SurveyQuestion> sq = new List<SurveyQuestion>(testsur.Questions);

                QuestionList = new List<string>();
                foreach (SurveyQuestion q in sq)
                {
                    QuestionList.Add(q.Question);
                }
            }

            Instance = this;
        }
    }
}