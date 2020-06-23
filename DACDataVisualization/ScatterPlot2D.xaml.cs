﻿using System;
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

        public ScatterPlot2D()
        {
            InitializeComponent();
        }

        // originX and originY are offsets from LL corner of canvas where 0 <= oX, oY <= 1
        public void SetAxes(double x0, double x1, double y0, double y1, AxesPreferences2D ap)
        {
            double adjCanvasWidth = PlotCanvas.ActualWidth - 2 * ap.PlotPadding;
            double adjCanvasHeight = PlotCanvas.ActualHeight - 2 * ap.PlotPadding;
            scaleX = (adjCanvasWidth) / (x1 - x0);
            scaleY = (adjCanvasHeight) / (y1 - y0);
            originX = (0 - x0) / (x1 - x0) * adjCanvasWidth + ap.PlotPadding;
            originY = (0 - y0) / (y1 - y0) * adjCanvasHeight + ap.PlotPadding;

            HorizontalAxis = new Line()
            {
                X1 = ap.PlotPadding,
                X2 = PlotCanvas.ActualWidth - ap.PlotPadding,
                Y1 = originY,
                Y2 = originY,
                Stroke = ap.HorizontalAxisBrush,
                StrokeThickness = ap.HorizontalAxisThickness
            };
            _ = PlotCanvas.Children.Add(HorizontalAxis);
            VerticalAxis = new Line()
            {
                X1 = originX,
                X2 = originX,
                Y1 = ap.PlotPadding,
                Y2 = PlotCanvas.ActualHeight - ap.PlotPadding,
                Stroke = ap.VerticalAxisBrush,
                StrokeThickness = ap.VerticalAxisThickness
            };
            _ = PlotCanvas.Children.Add(VerticalAxis);

            // THIS IS BEGGING FOR AN ARROWHEAD OR AXISENDDECORATION CLASS.
            // SCALING ARROWHEADS BY AXIS LENGTH WHEN X AND Y HAVE DIFFERENT LENGTHS
            // CAN LEAD TO FUNNY-LOOKING AXES. KEEP AN EYE ON THIS.
            if (ap.ArrowStyle == 1)
            {
                double arrowheadWidthScalingFactor = 60;
                double arrowheadHeightScalingFactor = 35;
                double YLength = VerticalAxis.Y2 - VerticalAxis.Y1;
                // verticalArrowLeft
                _ = PlotCanvas.Children.Add(new Line()
                {
                    X1 = VerticalAxis.X1 - YLength / arrowheadWidthScalingFactor,
                    X2 = VerticalAxis.X1,
                    Y1 = VerticalAxis.Y2 - YLength / arrowheadHeightScalingFactor,
                    Y2 = VerticalAxis.Y2,
                    Stroke = ap.VerticalAxisBrush,
                    StrokeThickness = ap.VerticalAxisThickness
                });
                // verticalArrowRight
                _ = PlotCanvas.Children.Add(new Line()
                {
                    X1 = VerticalAxis.X1 + YLength / arrowheadWidthScalingFactor,
                    X2 = VerticalAxis.X1,
                    Y1 = VerticalAxis.Y2 - YLength / arrowheadHeightScalingFactor,
                    Y2 = VerticalAxis.Y2,
                    Stroke = ap.VerticalAxisBrush,
                    StrokeThickness = ap.VerticalAxisThickness
                });
                double XLength = HorizontalAxis.X2 - HorizontalAxis.X1;
                // horizontalArrowTop
                _ = PlotCanvas.Children.Add(new Line()
                {
                    X1 = HorizontalAxis.X2 - XLength / arrowheadHeightScalingFactor,
                    X2 = HorizontalAxis.X2,
                    Y1 = HorizontalAxis.Y2 + XLength / arrowheadWidthScalingFactor,
                    Y2 = HorizontalAxis.Y2,
                    Stroke = ap.HorizontalAxisBrush,
                    StrokeThickness = ap.HorizontalAxisThickness
                });
                // horizontalArrowBottom
                _ = PlotCanvas.Children.Add(new Line()
                {
                    X1 = HorizontalAxis.X2 - XLength / arrowheadHeightScalingFactor,
                    X2 = HorizontalAxis.X2,
                    Y1 = HorizontalAxis.Y2 - XLength / arrowheadWidthScalingFactor,
                    Y2 = HorizontalAxis.Y2,
                    Stroke = ap.HorizontalAxisBrush,
                    StrokeThickness = ap.HorizontalAxisThickness
                });
            }

            DrawGridLines();
        }

        // THIS MOST BASIC VERSION NEEDS A LOT OF WORK. IT NEEDS A GRIDLINESPREFERENCES OBJECT FOR STARTERS.
        // IT ALSO NEEDS TO BE ABLE TO HANDLE A PRESET NUMBER OF LINES IN EACH DIMENSION WHICH CAN BE DIFFERENT.
        // IT ALSO NEEDS TO BE ABLE TO DRAW LINES INDICATED BY A XDIM ARRAY OF INTERCEPTS AND A YDIM ONE.
        // PERHAPS THE THING TO DO IS HAVE ONE DRAWGRIDLINES(DOUBLE[] XINTERCEPTS, DOUBLE[] YINTERCEPTS) WITH
        // DIFFERENT WRAPPERS WHICH CALCULATE THE INTERCEPTS DEPENDING ON THE INPUTS AND WHATNOT.
        private void DrawGridLines()
        {
            Brush b = new SolidColorBrush { Color = Color.FromArgb(70, 0x77, 0x88, 0x99) };
            double t = 1;
            for (int idx = 1; idx < 9; ++idx)
            {
                double xVal = originX + scaleX * idx;
                Line gridLine = new Line
                {
                    Stroke = b,
                    StrokeThickness = t,
                    X1 = xVal,
                    X2 = xVal,
                    Y1 = VerticalAxis.Y1,
                    Y2 = VerticalAxis.Y2
                };
                _ = PlotCanvas.Children.Add(gridLine);
            }
            for (int idx = 10; idx < 90; idx += 10)
            {
                double yVal = originY + scaleY * idx;
                Line gridLine = new Line
                {
                    Stroke = b,
                    StrokeThickness = t,
                    X1 = HorizontalAxis.X1,
                    X2 = HorizontalAxis.X2,
                    Y1 = yVal,
                    Y2 = yVal
                };
                _ = PlotCanvas.Children.Add(gridLine);
            }
        }

        public void PlotCurve2D(PointCollection points)
        {
            TranslatePoints2D(points, out PointCollection translatedPoints);

            Polyline curve = new Polyline
            {
                Points = translatedPoints,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1
            };
            _ = PlotCanvas.Children.Add(curve);
        }

        public void ClearPlotArea() => PlotCanvas.Children.Clear();

        public void PlotPoints2D(PointCollection points, DataPointPreferences dpp)
        {
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
        }

        private void TranslatePoints2D(PointCollection points, out PointCollection translatedPoints, double xOffset = 0, double yOffset = 0)
        {
            translatedPoints = new PointCollection();
            for (int idx = 0; idx < points.Count; ++idx)
                translatedPoints.Add(new Point()
                {
                    X = originX + (scaleX * points[idx].X) - xOffset / 2,
                    Y = originY + (scaleY * points[idx].Y) - yOffset / 2
                });
        }

        private void PlotCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double mouseX = e.GetPosition((IInputElement)sender).X;
            double mouseY = e.GetPosition((IInputElement)sender).Y;

            double? minR = null;
            double closestX = mouseX;
            double closestY = mouseY;
            foreach (UIElement elmt in PlotCanvas.Children)
            {
                if (elmt is Ellipse ell)
                {
                    double elmtX = Canvas.GetLeft(elmt);
                    double elmtY = Canvas.GetTop(elmt);
                    if (double.IsNaN(elmtX) || double.IsNaN(elmtY)) continue;
                    double distX = mouseX - elmtX;
                    double distY = mouseY - elmtY;
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
            string coords = "X-Coordinate: " + untranslatedX + "\n" + "Y-Coordinate: " + untranslatedY;
            MessageBox.Show(coords);
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            //{
            //    return;
            //}
            //ClearPlotArea();
        }
    }
}