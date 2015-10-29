using System;
using System.Collections;
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
using DataAccess;
using DataModel;

namespace MinimapTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Scenario Scenario { get; set; }
        //private string miniPath = @"C:\public\minimap.png";

        public MainWindow()
        {
            InitializeComponent();

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                this.Scenario = unitOfWork.ScenarioRepository.Get(s => s.Name == "Balance").FirstOrDefault();
                if (this.Scenario == null)
                {
                    throw new ArgumentException("Balance Scenario existiert nicht");
                }
                Canvas.DataContext = this.Scenario;
            }
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ImageSource backgroundImage = ((ImageBrush)Canvas.Background).ImageSource;
            Canvas.Width = backgroundImage.Width;
            Canvas.Height = backgroundImage.Height;
            this.Width = 800;
            this.Height = 600;

            IEnumerable<PositionData> positions = null;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                positions = unitOfWork.PositionDataRepository.Get(p => p.Test.ID == 1).OrderBy(p => p.Time).ToList();
            }
            
            double scale = Scenario.Scale;
            Point zero = new Point(Scenario.ZeroX, Scenario.ZeroY);
            bool first = true;
            
            PathFigure pathFigure = new PathFigure { Segments = new PathSegmentCollection() };
            Path myPath = new Path
            {
                Stroke = Brushes.Red,
                StrokeThickness = 5,
                Data = new PathGeometry { Figures = new PathFigureCollection { pathFigure } }
            };
            Canvas.Children.Add(myPath);





            foreach (PositionData position in positions)
            {
                if (first)
                {
                    Point pos = new Point(position.X * scale + zero.X, position.Y * scale + zero.Y);

                    double width = 10;
                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = new SolidColorBrush(Colors.Magenta),
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1,
                        Width = width,
                        Height = width
                    };
                    Canvas.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, pos.X - (width / 2.0));
                    Canvas.SetTop(ellipse, pos.Y - (width / 2.0));

                    pathFigure.StartPoint = pos;

                    first = false;
                }
                else
                {
                    Point newPoint = new Point(position.X * scale + zero.X, position.Y * scale + zero.Y);

                    pathFigure.Segments.Add(new LineSegment(newPoint, true));
                }
            }

        }
    }
}
