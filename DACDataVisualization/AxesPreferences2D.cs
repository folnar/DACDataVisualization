using System.Windows.Media;

namespace DACDataVisualization
{
    public class AxesPreferences2D
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
        public XAxisLabel XLabel { get; set; }
        public YAxisLabel YLabel { get; set; }

        private AxesPreferences2D() { }

        public static AxesPreferences2D CreateObject(Color c, double t, XAxisLabel xlabel = null, YAxisLabel ylabel = null)
        {
            AxesPreferences2D ap = new AxesPreferences2D
            {
                Brush = new SolidColorBrush
                {
                    Color = c
                },
                StrokeThickness = t,
                XLabel = xlabel,
                YLabel = ylabel
            };

            return ap;
        }

        public static AxesPreferences2D CreateObject(Color colorX, Color colorY, double thicknessX, double thicknessY, double plotpadding, int arrowStyle, XAxisLabel xlabel = null, YAxisLabel ylabel = null)
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
                ArrowStyle = arrowStyle,
                XLabel = xlabel,
                YLabel = ylabel
            };

            return ap;
        }
    }
}
