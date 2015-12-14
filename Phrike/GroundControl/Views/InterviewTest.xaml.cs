// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterviewTest.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for InterviewTest.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataModel;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for InterviewTest.xaml
    /// </summary>
    public partial class InterviewTest : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewTest"/> class.
        /// </summary>
        public InterviewTest()
        {
            AnswerList = new List<SurveyAnswer>();
            InitializeComponent();
        }

        public List<SurveyAnswer> AnswerList { get; set; }

        /// <summary>
        /// The button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
           // this.AnswerList = this.GetInterviewResult();
           this.GetInterviewResult();
        }

        /// <summary>
        /// The get interview result.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private void GetInterviewResult()
        {
            var grid = InterviewGrid;
            List<SurveyAnswer> result = new List<SurveyAnswer>();
            int i = 1;
            foreach (RadioButton rb in FindVisualChildren<RadioButton>(grid))
            {
                Console.WriteLine(rb.IsChecked);
            }
        }

        /// <summary>
        /// The find visual children.
        /// </summary>
        /// <param name="depObj">
        /// The dep obj.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {               
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// The radio button_ checked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        //private void RadioButtonChecked(object sender, RoutedEventArgs e)
        //{          
        //    RadioButton rb = (RadioButton)sender;

        //    if (rb.IsChecked == true)
        //    {
        //        Console.WriteLine(rb.IsChecked + "1");
        //    }
        //}
    }
}
