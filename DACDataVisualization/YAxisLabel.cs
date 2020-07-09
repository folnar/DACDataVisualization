using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DACDataVisualization
{
    internal class YAxisLabel : TextBlock
    {
        private double YOffsetPct { get; set; }
        private double XOffsetPixels { get; set; }

        private YAxisLabel() { }

        public static YAxisLabel NewAxisLabel(string label, double yoffsetPct, double xoffsetPixels, Brush foreground, Brush background, FontFamily font, double fontsize)
        {
            return new YAxisLabel()
            {
                Text = label,
                Foreground = foreground,
                Background = background,
                FontFamily = font,
                FontSize = fontsize,
                RenderTransform = new ScaleTransform(1, -1, 0.5, 0.5),
                YOffsetPct = yoffsetPct,
                XOffsetPixels = xoffsetPixels
            };
        }

        internal static YAxisLabel NewAxisLabel(string label, double yoffsetPct, int xoffsetPixels, LabelPreferences xlp)
        {
            return new YAxisLabel()
            {
                Text = label,
                Foreground = xlp.Foreground,
                Background = xlp.Background,
                FontFamily = xlp.Font,
                FontSize = xlp.Fontsize,
                FontStyle = xlp.Style,
                FontWeight = xlp.Weight,
                RenderTransform = new ScaleTransform(1, -1, 0.5, 0.5),
                YOffsetPct = yoffsetPct,
                XOffsetPixels = xoffsetPixels
            };
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
