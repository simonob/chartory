using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Chartory.Charting.Extensions;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chartory
{
    class PieSegment : Windows.UI.Xaml.Shapes.Path
    {
        public Point CenterPoint { get; set; }
        public Double Radius { get; set; }
        public Double StartAngle { get; set; }
        public Double EndAngle { get; set; }

        public Double InnerRadius { get; set; }

        public bool IsExploaded { get; set; }
        public Double ExplosionRadius { get; set; }
        public Double OffsetRadius { get; set; }

        public Double Value { get; set; }
        public Double Percentage { get; set; }

        public object DataItem { get; set; }
        public string Text { get; set; }

        public int PieceNumber { get; set; }

        private bool RenderAsCircle { get { return StartAngle == 0 && EndAngle == 360; } }

        public Double CurrentOffset
        {
            get { return (Double)GetValue(CurrentOffsetProperty); }
            set { SetValue(CurrentOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentOffsetProperty =
            DependencyProperty.Register("CurrentOffset", typeof(Double), typeof(PieSegment), new PropertyMetadata(0, (obj, e) =>
                {
                    if (e.NewValue != e.OldValue)
                    {
                        var pieSeg = (PieSegment)obj;
                        pieSeg.buildPath();
                    }

                }));

        private Storyboard _popOutstoryboard;

        public PieSegment()
        {
            this.Loaded += PieSegment_Loaded;

            this.RenderTransform = new TranslateTransform();

            CurrentOffset = OffsetRadius;
        }

        internal void PopOut()
        {
            if (_popOutstoryboard != null)
                _popOutstoryboard.Begin();
        }

        internal void PopIn()
        {
            ((TranslateTransform)this.RenderTransform).X = 0;
            ((TranslateTransform)this.RenderTransform).Y = 0;
        }

        void PieSegment_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            buildPath();

            if (!RenderAsCircle)
                createPopOutStoryboard();

            ToolTipService.SetToolTip(this, new ToolTip()
            {
                BorderThickness = new Thickness(),
                Padding = new Thickness(),
                Content = new PieSegmentLabel() 
                {
                    Value = this.Value,
                    Percentage = this.Percentage,
                    Text = this.Text,
                    DataItem = this.DataItem,
                    Brush = this.Fill
                }
            });

        }

        void createPopOutStoryboard()
        {
            var startingCenter = calculateStartingCenterPoint();
            var explodedCenter = calculateExplodedCenterPoint();

            DoubleAnimation xAnim = new DoubleAnimation();
            xAnim.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            xAnim.From = 0;
            xAnim.To = explodedCenter.X - startingCenter.X;

            Storyboard.SetTarget(xAnim, this.RenderTransform);
            Storyboard.SetTargetProperty(xAnim, "X");

            DoubleAnimation yAnim = new DoubleAnimation();
            yAnim.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            yAnim.From = 0;
            yAnim.To = explodedCenter.Y - startingCenter.Y;

            Storyboard.SetTarget(yAnim, this.RenderTransform);
            Storyboard.SetTargetProperty(yAnim, "Y");

            _popOutstoryboard = new Storyboard();
            _popOutstoryboard.Children.Add(xAnim);
            _popOutstoryboard.Children.Add(yAnim);

        }

        private Point calculateStartingCenterPoint()
        {
            return calculateCenterPoint(OffsetRadius);
        }

        private Point calculateExplodedCenterPoint()
        {
            return calculateCenterPoint(ExplosionRadius);
        }

        private Point calculateCenterPoint(Double offset)
        {
            var point = CenterPoint;

            double centerLineAngle = (StartAngle + EndAngle) / 2;
            point = Utils.ComputeCartesianCoordinate(centerLineAngle, offset).Offset(CenterPoint.X, CenterPoint.Y);

            return point;
        }

        private void buildPath()
        {
            Point centerPoint;
            if (IsExploaded)
                centerPoint = calculateExplodedCenterPoint();
            else
                centerPoint = calculateStartingCenterPoint();

            if (RenderAsCircle)
               this.Data = buildSingleItemPath(centerPoint);
            else
            {
                var pf = new PathFigure();
                if (InnerRadius == 0)
                    pf.StartPoint = centerPoint;
                else
                    pf.StartPoint = Utils.ComputeCartesianCoordinate(StartAngle, InnerRadius).Offset(centerPoint.X, centerPoint.Y);

                pf.Segments = new PathSegmentCollection();

                //add start line
                var startLine = new LineSegment();
                startLine.Point = Utils.ComputeCartesianCoordinate(StartAngle, Radius).Offset(centerPoint.X, centerPoint.Y);
                pf.Segments.Add(startLine);

                //add arc segment
                var arc = new ArcSegment()
                {
                    Point = Utils.ComputeCartesianCoordinate(EndAngle, Radius).Offset(centerPoint.X, centerPoint.Y),
                    Size = new Windows.Foundation.Size(Radius, Radius),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = (EndAngle - StartAngle > 180),
                };
                pf.Segments.Add(arc);

                //add closing line
                var endLine = new LineSegment();
                if (InnerRadius == 0)
                    endLine.Point = centerPoint;
                else
                    endLine.Point = Utils.ComputeCartesianCoordinate(EndAngle, InnerRadius).Offset(centerPoint.X, centerPoint.Y);

                pf.Segments.Add(endLine);

                if (InnerRadius != 0)
                {
                    //add inner arc segment
                    var innerArc = new ArcSegment()
                    {
                        Point = Utils.ComputeCartesianCoordinate(StartAngle, InnerRadius).Offset(centerPoint.X, centerPoint.Y),
                        Size = new Windows.Foundation.Size(InnerRadius, InnerRadius),
                        SweepDirection = SweepDirection.Counterclockwise,
                        IsLargeArc = (EndAngle - StartAngle > 180)
                    };
                    pf.Segments.Add(innerArc);

                }

                this.Data = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                    {
                        pf
                    }
                };
            }

        }

        private Geometry buildSingleItemPath(Point centerPoint)
        {
            GeometryGroup gg = new GeometryGroup();

            EllipseGeometry outerEllipse = new EllipseGeometry();
            outerEllipse.Center = centerPoint;
            outerEllipse.RadiusX = Radius;
            outerEllipse.RadiusY = Radius;

            gg.Children.Add(outerEllipse);

            if (InnerRadius != 0)
            {
                EllipseGeometry innerEllipse = new EllipseGeometry();
                innerEllipse.Center = centerPoint;
                innerEllipse.RadiusX = InnerRadius;
                innerEllipse.RadiusY = InnerRadius;

                gg.Children.Add(innerEllipse);
            }

            return gg;

        }

    }
}
