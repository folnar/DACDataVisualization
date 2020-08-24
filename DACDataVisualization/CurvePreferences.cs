using System.Windows.Media;

namespace DACDataVisualization
{
    public class CurvePreferences : IPlotPreferences
    {
        public Brush Brush { get; set; }
        public double StrokeThickness { get; set; }
        public DoubleCollection DashArray { get; set; }
        public PenLineCap DashCap { get; set; }
        public double DashOffset { get; set; }

        private CurvePreferences() { }

        public static CurvePreferences NewCurvePreferences(Brush colorBrush, double strokeThickness, DoubleCollection dashesArray, PenLineCap dashCaps = PenLineCap.Flat, double dashOffset = 0)
        {
            return new CurvePreferences
            {
                Brush = colorBrush,
                StrokeThickness = strokeThickness,
                DashArray = dashesArray,
                DashCap = dashCaps,
                DashOffset = dashOffset
            };
        }

        public static CurvePreferences NewCurvePreferences(Brush colorBrush, double strokeThickness)
        {
            CurvePreferences cp = new CurvePreferences()
            {
                Brush = colorBrush,
                StrokeThickness = strokeThickness
            };
            cp.DashArray = new DoubleCollection() { 1 };
            cp.DashCap = PenLineCap.Flat;
            cp.DashOffset = 0;
            return cp;
        }
    }
}
