using System.Windows.Media;

namespace DACDataVisualization
{
    public class CurvePreferences
    {
        public Brush StrokeBrush { get; set; }
        public double Thickness { get; set; }
        public DoubleCollection DashArray { get; set; }
        public PenLineCap DashCap { get; set; }
        public double DashOffset { get; set; }

        private CurvePreferences() { }

        public static CurvePreferences NewCurvePreferences(Brush colorBrush, double strokeThickness, DoubleCollection dashesArray, PenLineCap dashCaps = PenLineCap.Flat, double dashOffset = 0)
        {
            return new CurvePreferences
            {
                StrokeBrush = colorBrush,
                Thickness = strokeThickness,
                DashArray = dashesArray,
                DashCap = dashCaps,
                DashOffset = dashOffset
            };
        }

        public static CurvePreferences NewCurvePreferences(Brush colorBrush, double strokeThickness)
        {
            CurvePreferences cp = new CurvePreferences()
            {
                StrokeBrush = colorBrush,
                Thickness = strokeThickness
            };
            cp.DashArray = new DoubleCollection() { 1 };
            cp.DashCap = PenLineCap.Flat;
            cp.DashOffset = 0;
            return cp;
        }
    }
}
