using System.Windows.Controls;
using System.Windows.Media;

namespace DACDataVisualization
{
    class XAxisLabel : TextBlock
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

        public void DrawLabel(Canvas PlotCanvas)
        {
            //Line axis = PlotCanvas.HorizontalAxis;
            //Canvas.SetLeft(this, HorizontalAxis.X2 - (HorizontalAxis.X2 - HorizontalAxis.X1) * XOffsetPct);
            //Canvas.SetTop(this, HorizontalAxis.Y1 - YOffsetPixels);
        }
    }
}
