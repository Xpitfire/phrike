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
using Phrike.GroundControl.ViewModels;
using System.Globalization;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for TestResult.xaml
    /// </summary>
    public partial class TestArchive : UserControl
    {
        public TestArchive()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                var context = new TestArchiveViewModel();
                this.DataContext = context;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTest.ItemsSource);
                view.Filter = Filter;
                context.FilterChanged += () => view.Refresh();
            };
        }

        private bool Filter(object o)
        {
            var x = this.DataContext as TestArchiveViewModel;
            return x.Filter(o);
        }
        

    }
}
