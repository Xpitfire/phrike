using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Start : UserControl
    {
        public Start()
        {
            InitializeComponent();
        }

        private void ButtonNewStresstest_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("New Stress-Test!");
            foreach (var resource in Resources)
            {
                Console.WriteLine(resource);
            }
        }
    }
}
