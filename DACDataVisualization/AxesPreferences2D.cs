using System.Windows.Media;

namespace DACDataVisualization
{
    public class AxesPreferences2D// : IPlotPreferences
    {
        public double PlotPadding;
        public SolidColorBrush VerticalAxisBrush { get; set; }
        public SolidColorBrush HorizontalAxisBrush { get; set; }
        private SolidColorBrush Brush
        {
            get => Brush;
            set { VerticalAxisBrush = value; HorizontalAxisBrush = value; }
        }
        public double VerticalAxisThickness { get; set; }
        public double HorizontalAxisThickness { get; set; }
        private double StrokeThickness
        {
            get => StrokeThickness;
            set { VerticalAxisThickness = value; HorizontalAxisThickness = value; }
        }
        public int ArrowStyle { get; set; }

        private AxesPreferences2D() { }

        public static AxesPreferences2D CreateObject(Color c, double t)
        {
            AxesPreferences2D ap = new AxesPreferences2D
            {
                Brush = new SolidColorBrush
                {
                    Color = c
                },
                StrokeThickness = t
            };

            return ap;
        }

        public static AxesPreferences2D CreateObject(Color colorX, Color colorY, double thicknessX, double thicknessY, double plotpadding, int arrowStyle)
        {
            AxesPreferences2D ap = new AxesPreferences2D
            {
                VerticalAxisBrush = new SolidColorBrush
                {
                    Color = colorX
                },
                HorizontalAxisBrush = new SolidColorBrush
                {
                    Color = colorY
                },
                VerticalAxisThickness = thicknessX,
                HorizontalAxisThickness = thicknessY,
                PlotPadding = plotpadding,
                ArrowStyle = arrowStyle
            };

            return ap;
        }
    }
}
