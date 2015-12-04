using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            this.InitializeComponent();
        }

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
            Console.WriteLine(@"hallo");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.IsChecked == true)
            {
                Console.WriteLine(rb.IsChecked);
            }
        }
    }
}
