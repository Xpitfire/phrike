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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using DataAccess;
using DataModel;

using Phrike.GroundControl.Helper;

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

        public SurveyVM CurrentSurvey { get; set; }

        public SurveyQuestionCollectionVM QuestionCollectionVM => new SurveyQuestionCollectionVM();

        public List<string> SurveyAnsList { get; set; }


        ///// <summary>
        ///// Gets or sets the survey name.
        ///// </summary>
        //public string SurveyName { get; set; }

        ///// <summary>
        ///// Gets or sets the survey.
        ///// </summary>
        //private static Survey Survey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewTestViewModel"/> class.
        /// </summary>
        public InterviewTestViewModel()
        {
            if (DataLoadHelper.IsLoadDataActive())
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    CurrentSurvey = new SurveyVM(unitOfWork.SurveyRepository.Get().FirstOrDefault());
                }
                this.QuestionCollectionVM.LoadSurveyQuestions();
                
                if (CurrentSurvey != null)
                    this.QuestionCollectionVM.LoadSurveyQuestions(CurrentSurvey.Id);
            }

            SurveyAnsList = new List<string>
                            {
                                SurveyAnswer.Perfect.ToString(),
                                SurveyAnswer.Good.ToString(),
                                SurveyAnswer.Gratifying.ToString(),
                                SurveyAnswer.Bad.ToString(),
                                SurveyAnswer.Worst.ToString()
                            };

            foreach (var q in this.QuestionCollectionVM.SurveyQuestions)
            {
                Console.WriteLine(q);
            }
        }

        public void SaveData(List<SurveyResult> resultList)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                int i = 0; // !!!!
                SurveyQuestion[] questionList = unitOfWork.SurveyQuestionRepository.Get().ToArray();
                List<SurveyQuestion> questionList2 = (List<SurveyQuestion>)unitOfWork.SurveyQuestionRepository.Get();
                // TODO: Get correct test!!!!
                Test test = unitOfWork.TestRepository.Get().FirstOrDefault();
                
                if (test != null)
                {
                    foreach (SurveyResult result in resultList)
                    {
                        result.Test = test;
                        result.SurveyQuestion = questionList[i];
                        unitOfWork.SurveyResultRepository.Insert(result);
                        unitOfWork.Save();
                    }
                }
            }
        }
    }
}