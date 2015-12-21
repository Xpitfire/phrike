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
using System.Windows;

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

        public int CurrentTestId { get; set; }

        public bool IsEditable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewTestViewModel"/> class.
        /// </summary>
        public InterviewTestViewModel(int testId, bool isEditable = true)
        {
            CurrentTestId = testId;
            IsEditable = isEditable;
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
                                SurveyAnswerHelper.ToString(SurveyAnswer.Perfect),
                                SurveyAnswerHelper.ToString(SurveyAnswer.Good),
                                SurveyAnswerHelper.ToString(SurveyAnswer.Gratified),
                                SurveyAnswerHelper.ToString(SurveyAnswer.Bad),
                                SurveyAnswerHelper.ToString(SurveyAnswer.Worst),
                            };

            foreach (var q in this.QuestionCollectionVM.SurveyQuestions)
            {
                Console.WriteLine(q);
            }
        }

        public SurveyResult GetCurrentAnswer()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                Test test = unitOfWork.TestRepository.GetByID(CurrentTestId);
                return test?.SurveyResult.FirstOrDefault();
            }
        }

        public void SaveData(List<SurveyResult> resultList)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                int i = 0; // !!!!
                SurveyQuestion[] questionList = unitOfWork.SurveyQuestionRepository.Get().ToArray();
                List<SurveyQuestion> questionList2 = (List<SurveyQuestion>)unitOfWork.SurveyQuestionRepository.Get();
                Test test = unitOfWork.TestRepository.GetByID(CurrentTestId);

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