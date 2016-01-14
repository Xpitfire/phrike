using System.Windows.Controls;

using DataAccess;
using DataModel;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Data;
using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl.Views {
    /// <summary>
    /// Interaktionslogik für Analysis.xaml
    /// </summary>
    public partial class Analysis : UserControl
    {
        public Scenario Scenario { get; set; }

        public int TestId { get; set; }

        public Analysis()
        {
            InitializeComponent();            
        }

        private void Analysis_OnLoaded(object sender, RoutedEventArgs e)
        {
            AnalysisViewModel vm = (AnalysisViewModel)DataContext;
            Scenario = vm.CurrentTest.Scenario;
            ImageSource backgroundImage = ((ImageBrush)Canvas.Background).ImageSource;
            Canvas.Width = backgroundImage.Width;
            Canvas.Height = backgroundImage.Height;
            IEnumerable<PositionData> positions = null;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                positions = unitOfWork.PositionDataRepository.Get(p => p.Test.Id == vm.CurrentTestId).OrderBy(p => p.Time).ToList();
            }

            if(positions.Count() == 0)
            {
                return;
            }

            double scale = Scenario.Scale;
            Point zero = new Point(Scenario.ZeroX, Scenario.ZeroY);
            bool first = true;

            PathFigure pathFigure = new PathFigure { Segments = new PathSegmentCollection() };
            Path myPath = new Path
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 25,
                Data = new PathGeometry { Figures = new PathFigureCollection { pathFigure } }
            };
            Canvas.Children.Add(myPath);

            foreach (PositionData position in positions)
            {
                if (first)
                {
                    Point pos = new Point(position.X * scale + zero.X, position.Y * scale + zero.Y);

                    double width = 50;
                    pos = CreatePoint(pos, width, Colors.Lime);

                    pathFigure.StartPoint = pos;

                    first = false;
                }
                else
                {
                    Point newPoint = new Point(position.X * scale + zero.X, position.Y * scale + zero.Y);

                    pathFigure.Segments.Add(new LineSegment(newPoint, true));
                }
            }
            Point lastPoint = ((LineSegment)pathFigure.Segments.Last()).Point;
            CreatePoint(lastPoint, 50, Colors.Yellow);
        }


        private Point CreatePoint(Point pos, double width, Color color)
        {
            Ellipse ellipse = new Ellipse()
            {
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                Width = width,
                Height = width
            };
            Canvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, pos.X - (width / 2.0));
            Canvas.SetTop(ellipse, pos.Y - (width / 2.0));
            return pos;
        }
    }
}
