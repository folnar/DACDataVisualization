using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DACDataVisualization
{
    public class YAxisLabel : TextBlock
    {
        private double YOffsetPct { get; set; }
        private double XOffsetPixels { get; set; }

        private YAxisLabel() { }

        public static YAxisLabel NewAxisLabel(string label, double yoffsetPct, double xoffsetPixels, Brush foreground, Brush background, FontFamily font, double fontsize, LabelOrientations orientation)
        {
            YAxisLabel yal = new YAxisLabel()
            {
                Text = label,
                Foreground = foreground,
                Background = background,
                FontFamily = font,
                FontSize = fontsize,
                YOffsetPct = yoffsetPct,
                XOffsetPixels = xoffsetPixels
            };
            ScaleTransform st = new ScaleTransform(1, -1, 0.5, 0.5);
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(st);
            if (orientation == LabelOrientations.VerticalBottomToTop)
                tg.Children.Add(new RotateTransform(90));
            if (orientation == LabelOrientations.VerticalTopToBottom)
                tg.Children.Add(new RotateTransform(-90));
            yal.RenderTransform = tg;

            return yal;
        }

        public static YAxisLabel NewAxisLabel(string label, double yoffsetPct, int xoffsetPixels, LabelPreferences ylp)
        {
            YAxisLabel yal = new YAxisLabel()
            {
                Text = label,
                Foreground = ylp.Foreground,
                Background = ylp.Background,
                FontFamily = ylp.Font,
                FontSize = ylp.Fontsize,
                FontStyle = ylp.Style,
                FontWeight = ylp.Weight,
                YOffsetPct = yoffsetPct,
                XOffsetPixels = xoffsetPixels
            };
            ScaleTransform st = new ScaleTransform(1, -1, 0.5, 0.5);
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(st);
            if (ylp.Orientation == LabelOrientations.VerticalBottomToTop)
            {
                yal.RenderTransformOrigin = new System.Windows.Point(1, 1);
                tg.Children.Add(new RotateTransform(90));
            }
            if (ylp.Orientation == LabelOrientations.VerticalTopToBottom)
                tg.Children.Add(new RotateTransform(-90));
            yal.RenderTransform = tg;

            return yal;
        }

        internal double CanvasLeftPosition(Line verticalAxis)
        {
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return verticalAxis.X1 - XOffsetPixels - DesiredSize.Width;
        }

        internal double CanvasTopPosition(Line verticalAxis)
        {
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return (verticalAxis.Y2 - (verticalAxis.Y2 - verticalAxis.Y1) * YOffsetPct) + DesiredSize.Height;
        }
    }
}
