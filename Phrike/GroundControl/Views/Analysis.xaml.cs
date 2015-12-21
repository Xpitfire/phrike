using System.Windows.Controls;

using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl.Views {
    /// <summary>
    /// Interaktionslogik für Analysis.xaml
    /// </summary>
    public partial class Analysis : UserControl
    {
        public Analysis(int testId)
        {
            InitializeComponent();
            DataContext = new AnalysisViewModel(testId);
        }
    }
}
