using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DACDataVisualization
{
    internal class XAxisLabel : TextBlock
    {
        private double XOffsetPct { get; set; }
        private double YOffsetPixels { get; set; }

        private XAxisLabel() { }

        public static XAxisLabel NewAxisLabel(string label, double xoffsetPct, double yoffsetPixels, Brush foreground, Brush background, FontFamily font, double fontsize)
        {
            return new XAxisLabel()
            {
                Text = label,
                Foreground = foreground,
                Background = background,
                FontFamily = font,
                FontSize = fontsize,
                RenderTransform = new ScaleTransform(1, -1, 0.5, 0.5),
                XOffsetPct = xoffsetPct,
                YOffsetPixels = yoffsetPixels
            };
        }

        internal static XAxisLabel NewAxisLabel(string label, double xoffsetPct, int yoffsetPixels, LabelPreferences xlp)
        {
            return new XAxisLabel()
            {
                Text = label,
                Foreground = xlp.Foreground,
                Background = xlp.Background,
                FontFamily = xlp.Font,
                FontSize = xlp.Fontsize,
                FontStyle = xlp.Style,
                FontWeight = xlp.Weight,
                RenderTransform = new ScaleTransform(1, -1, 0.5, 0.5),
                XOffsetPct = xoffsetPct,
                YOffsetPixels = yoffsetPixels
            };
        }

        internal double CanvasLeftPosition(Line horizontalAxis)
        {
            return horizontalAxis.X2 - (horizontalAxis.X2 - horizontalAxis.X1) * XOffsetPct;
        }

        internal double CanvasTopPosition(Line horizontalAxis)
        {
            return horizontalAxis.Y1 - YOffsetPixels + DesiredSize.Height;
        }
    }
}
