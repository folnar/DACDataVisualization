using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DACDataVisualization
{
    /// <summary>
    /// Interaction logic for ScatterPlot2D.xaml
    /// </summary>
    public partial class ScatterPlot2D : UserControl
    {
        private double scaleX;
        private double scaleY;
        private double originX;
        private double originY;

        private Line HorizontalAxis;
        private Line VerticalAxis;

        private GridLines PlotGridLines;

        private Point startZoomPosition;
        private double currXOffset;
        private double currYOffset;
        private AxesPreferences2D currAxesPrefs;
        private bool currDrawXAtY0Pref;
        private bool InZoomedMode;
        private object unzoomFunctionCall;
        private readonly Dictionary<string, (PointCollection, IPlotPreferences)> currPlotData;

        public ScatterPlot2D()
        {
            InitializeComponent();
            PlotGridLines = null;
            currPlotData = new Dictionary<string, (PointCollection, IPlotPreferences)>();
        }

        // originX and originY are offsets from LL corner of canvas where 0 <= oX, oY <= 1
        public void SetAxes(double x0, double x1, double y0, double y1, AxesPreferences2D ap = null, bool drawHorAxisAtY0 = false)
        {
            //if (!InZoomedMode)
            //    unzoomFunctionCall = Action<double,double,double,double,AxesPreferences2D,bool> SetAxes(x0, x1, y0, y1, ap, drawHorAxisAtY0);

            if (ap == null)
                ap = AxesPreferences2D.CreateObject(Colors.Black, Colors.Black, 1, 1, 0, 0);
            currAxesPrefs = ap;
            currDrawXAtY0Pref = drawHorAxisAtY0;

            double adjCanvasWidth = PlotCanvas.ActualWidth - 2 * currAxesPrefs.PlotPadding;
            double adjCanvasHeight = PlotCanvas.ActualHeight - 2 * currAxesPrefs.PlotPadding;
            scaleX = (adjCanvasWidth) / (x1 - x0);
            scaleY = (adjCanvasHeight) / (y1 - y0);
            originX = (0 - x0) / (x1 - x0) * adjCanvasWidth + currAxesPrefs.PlotPadding;
            originY = (0 - y0) / (y1 - y0) * adjCanvasHeight + currAxesPrefs.PlotPadding;

            HorizontalAxis = new Line()
            {
                X1 = currAxesPrefs.PlotPadding,
                X2 = PlotCanvas.ActualWidth - currAxesPrefs.PlotPadding,
                Y1 = drawHorAxisAtY0 ? originY : currAxesPrefs.PlotPadding,
                Y2 = drawHorAxisAtY0 ? originY : currAxesPrefs.PlotPadding,
                Stroke = currAxesPrefs.HorizontalAxisBrush,
                StrokeThickness = currAxesPrefs.HorizontalAxisThickness
            };
            PlotCanvas.Children.Add(HorizontalAxis);
            VerticalAxis = new Line()
            {
                X1 = currAxesPrefs.PlotPadding,
                X2 = currAxesPrefs.PlotPadding,
                Y1 = currAxesPrefs.PlotPadding,
                Y2 = PlotCanvas.ActualHeight - currAxesPrefs.PlotPadding,
                Stroke = currAxesPrefs.VerticalAxisBrush,
                StrokeThickness = currAxesPrefs.VerticalAxisThickness
            };
            PlotCanvas.Children.Add(VerticalAxis);

            // THIS IS BEGGING FOR AN ARROWHEAD OR AXISENDDECORATION CLASS.
            // SCALING ARROWHEADS BY AXIS LENGTH WHEN X AND Y HAVE DIFFERENT LENGTHS
            // CAN LEAD TO FUNNY-LOOKING AXES. KEEP AN EYE ON THIS.
            // WE ARE ALSO NOT ADDRESSING LEFT- AND DOWN-FACING ARROWS.
            if (currAxesPrefs.ArrowStyle == 1)
            {
                double arrowheadWidthScalingFactor = 60;
                double arrowheadHeightScalingFactor = 35;
                double YLength = VerticalAxis.Y2 - VerticalAxis.Y1;
                // verticalArrowLeft
                PlotCanvas.Children.Add(new Line()
                {
                    X1 = VerticalAxis.X1 - YLength / arrowheadWidthScalingFactor,
                    X2 = VerticalAxis.X1,
                    Y1 = VerticalAxis.Y2 - YLength / arrowheadHeightScalingFactor,
                    Y2 = VerticalAxis.Y2,
                    Stroke = currAxesPrefs.VerticalAxisBrush,
                    StrokeThickness = currAxesPrefs.VerticalAxisThickness
                });
                // verticalArrowRight
                PlotCanvas.Children.Add(new Line()
                {
                    X1 = VerticalAxis.X1 + YLength / arrowheadWidthScalingFactor,
                    X2 = VerticalAxis.X1,
                    Y1 = VerticalAxis.Y2 - YLength / arrowheadHeightScalingFactor,
                    Y2 = VerticalAxis.Y2,
                    Stroke = currAxesPrefs.VerticalAxisBrush,
                    StrokeThickness = currAxesPrefs.VerticalAxisThickness
                });
                double XLength = HorizontalAxis.X2 - HorizontalAxis.X1;
                // horizontalArrowTop
                PlotCanvas.Children.Add(new Line()
                {
                    X1 = HorizontalAxis.X2 - XLength / arrowheadHeightScalingFactor,
                    X2 = HorizontalAxis.X2,
                    Y1 = HorizontalAxis.Y2 + XLength / arrowheadWidthScalingFactor,
                    Y2 = HorizontalAxis.Y2,
                    Stroke = currAxesPrefs.HorizontalAxisBrush,
                    StrokeThickness = currAxesPrefs.HorizontalAxisThickness
                });
                // horizontalArrowBottom
                PlotCanvas.Children.Add(new Line()
                {
                    X1 = HorizontalAxis.X2 - XLength / arrowheadHeightScalingFactor,
                    X2 = HorizontalAxis.X2,
                    Y1 = HorizontalAxis.Y2 - XLength / arrowheadWidthScalingFactor,
                    Y2 = HorizontalAxis.Y2,
                    Stroke = currAxesPrefs.HorizontalAxisBrush,
                    StrokeThickness = currAxesPrefs.HorizontalAxisThickness
                });
            }

            if (currAxesPrefs.XLabel != null)
                DrawXAxisLabel(currAxesPrefs.XLabel);
            if (currAxesPrefs.YLabel != null)
                DrawYAxisLabel(currAxesPrefs.YLabel);
        }

        public void SetPlotGridLines(int numHorizontalLines, int numVerticalLines, CurvePreferences glcp = null)
        {
            if (glcp == null)
                glcp = CurvePreferences.NewCurvePreferences(new SolidColorBrush { Color = Color.FromArgb(70, 0x77, 0x88, 0x99) }, 1);
            PlotGridLines = GridLines.NewGridLines(glcp, HorizontalAxis.X1, HorizontalAxis.X2, VerticalAxis.Y1, VerticalAxis.Y2, numVerticalLines, numHorizontalLines);
            DrawPlotGridLines();
        }

        private void DrawPlotGridLines()
        {
            foreach (Line gridLine in PlotGridLines.GridLineSet)
                PlotCanvas.Children.Add(gridLine);
        }

        public void DrawXAxisLabel(XAxisLabel xal)
        {
            Canvas.SetLeft(xal, xal.CanvasLeftPosition(HorizontalAxis));
            Canvas.SetTop(xal, xal.CanvasTopPosition(HorizontalAxis));
            PlotCanvas.Children.Add(xal);
        }

        public void DrawYAxisLabel(YAxisLabel yal)
        {
            Canvas.SetLeft(yal, yal.CanvasLeftPosition(VerticalAxis));
            Canvas.SetTop(yal, yal.CanvasTopPosition(VerticalAxis));
            PlotCanvas.Children.Add(yal);
        }

        public void PlotCurve2D(string plotName, PointCollection points, CurvePreferences cp = null)
        {
            if (cp == null)
                cp = CurvePreferences.NewCurvePreferences(Brushes.Black, 1);

            TranslatePoints2D(points, out PointCollection translatedPoints);

            Polyline curve = new Polyline
            {
                Points = translatedPoints,
                Stroke = cp.Brush,
                StrokeThickness = cp.StrokeThickness,
                StrokeDashArray = cp.DashArray,
                StrokeDashCap = cp.DashCap,
                StrokeDashOffset = cp.DashOffset
            };
            PlotCanvas.Children.Add(curve);

            if (!currPlotData.ContainsKey(plotName))
                currPlotData.Add(plotName, (points, cp));
        }

        public void PlotPoints2D(string plotName, PointCollection points, DataPointPreferences dpp = null)
        {
            if (dpp == null)
                dpp = DataPointPreferences.CreateObject(Colors.Black, 1, 1, 1);

            TranslatePoints2D(points, out PointCollection translatedPoints, dpp.XDim, dpp.YDim);

            foreach (Point p in translatedPoints)
            {
                Ellipse e = new Ellipse
                {
                    Height = dpp.XDim,
                    Width = dpp.YDim,
                    StrokeThickness = dpp.StrokeThickness,
                    Stroke = dpp.Brush
                };
                PlotCanvas.Children.Add(e);

                Canvas.SetLeft(e, p.X);
                Canvas.SetTop(e, p.Y);
            }

            if (!currPlotData.ContainsKey(plotName))
                currPlotData.Add(plotName, (points, dpp));
        }

        private void RePlot()
        {
            foreach (string key in currPlotData.Keys)
            {
                PointCollection pc = currPlotData[key].Item1;
                IPlotPreferences ipp = currPlotData[key].Item2;
                if (ipp is DataPointPreferences)
                    PlotPoints2D(key, pc, ipp as DataPointPreferences);
                else if (ipp is CurvePreferences)
                    PlotCurve2D(key, pc, ipp as CurvePreferences);
            }
        }

        public void ClearPlotArea(bool clearPlotData = false)
        {
            PlotCanvas.Children.Clear();

            ZoomBox.Visibility = Visibility.Collapsed;
            PlotCanvas.Children.Add(ZoomBox);

            ScaleTransform st = new ScaleTransform(1, -1, 1, 1);
            PlotCanvas.LayoutTransform = st;

            if (clearPlotData)
                currPlotData.Clear();
        }

        private void TranslatePoints2D(PointCollection points, out PointCollection translatedPoints, double xOffset = 0, double yOffset = 0)
        {
            currXOffset = xOffset;
            currYOffset = yOffset;
            translatedPoints = new PointCollection();
            for (int idx = 0; idx < points.Count; ++idx)
                translatedPoints.Add(new Point()
                {
                    X = originX + (scaleX * points[idx].X) - xOffset / 2,
                    Y = originY + (scaleY * points[idx].Y) - yOffset / 2
                });
        }

        private void ShowCoordinatesBox(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(PlotCanvas);
            if (startZoomPosition == mousePosition)
            {
                double? minR = null;
                double closestX = mousePosition.X;
                double closestY = mousePosition.Y;
                foreach (UIElement elmt in PlotCanvas.Children)
                {
                    if (elmt is Ellipse ell)
                    {
                        double elmtX = Canvas.GetLeft(elmt);
                        double elmtY = Canvas.GetTop(elmt);
                        if (double.IsNaN(elmtX) || double.IsNaN(elmtY)) continue;
                        double distX = mousePosition.X - elmtX;
                        double distY = mousePosition.Y - elmtY;
                        double distR = Math.Sqrt(distX * distX + distY * distY);
                        if ((distR < minR || minR == null) && distR < scaleX * 0.2)
                        {
                            minR = distR;
                            closestX = elmtX + ell.Width / 2;
                            closestY = elmtY + ell.Height / 2;
                            if (distR < scaleX * 0.05) break;
                        }
                    }
                }

                double untranslatedX = (closestX - originX) / scaleX;
                double untranslatedY = (closestY - originY) / scaleY;
                string coords = $"X-Coordinate: {untranslatedX}\nY-Coordinate: {untranslatedY}\nX-Canvas: {closestX}\nY-Canvas: {closestY}";
                CoordsPopup.IsOpen = false;
                CoordsPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                CoordsPopupText.Text = coords;
                CoordsPopup.IsOpen = true;
            }
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded || Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                return;
            }
            ClearPlotArea();
        }

        private void PlotCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CoordsPopup.IsOpen = false;
            if (e.ChangedButton == MouseButton.Left)
            {
                startZoomPosition = e.GetPosition(PlotCanvas);
                PlotCanvas.CaptureMouse();
                Canvas.SetLeft(ZoomBox, startZoomPosition.X);
                Canvas.SetTop(ZoomBox, startZoomPosition.Y);
                ZoomBox.Width = 0;
                ZoomBox.Height = 0;
                ZoomBox.Visibility = Visibility.Visible;
            }
        }

        private void PlotCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (PlotCanvas.IsMouseCaptured)
            {
                Point mousePosition = e.GetPosition(PlotCanvas);

                if (startZoomPosition.X < mousePosition.X)
                {
                    Canvas.SetLeft(ZoomBox, startZoomPosition.X);
                    ZoomBox.Width = mousePosition.X - startZoomPosition.X;
                }
                else
                {
                    Canvas.SetLeft(ZoomBox, mousePosition.X);
                    ZoomBox.Width = startZoomPosition.X - mousePosition.X;
                }

                if (startZoomPosition.Y < mousePosition.Y)
                {
                    Canvas.SetTop(ZoomBox, startZoomPosition.Y);
                    ZoomBox.Height = mousePosition.Y - startZoomPosition.Y;
                }
                else
                {
                    Canvas.SetTop(ZoomBox, mousePosition.Y);
                    ZoomBox.Height = startZoomPosition.Y - mousePosition.Y;
                }
            }
        }

        private void PlotCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mouseUpPosition = e.GetPosition(PlotCanvas);

            PlotCanvas.ReleaseMouseCapture();

            if (e.ChangedButton == MouseButton.Right || !PlotCanvas.IsMouseOver)
            {
                CoordsPopup.IsOpen = false;
                ZoomBox.Visibility = Visibility.Collapsed;
            }
            else if (startZoomPosition == mouseUpPosition)
            {
                ShowCoordinatesBox(sender, e);
            }
            else
            {
                InZoomedMode = true;
                double[] xs = new double[] { UnTranslateXPoint2D(startZoomPosition.X), UnTranslateXPoint2D(mouseUpPosition.X) };
                double[] ys = new double[] { UnTranslateYPoint2D(startZoomPosition.Y), UnTranslateYPoint2D(mouseUpPosition.Y) };
                ClearPlotArea();
                SetAxes(xs.Min(), xs.Max(), ys.Min(), ys.Max(), currAxesPrefs, currDrawXAtY0Pref);
                RePlot();
            }
        }

        private double UnTranslateXPoint2D(double Tx) => (Tx - originX + (currXOffset / 2)) / scaleX;
        private double UnTranslateYPoint2D(double Ty) => (Ty - originY + (currYOffset / 2)) / scaleY;
    }
}
