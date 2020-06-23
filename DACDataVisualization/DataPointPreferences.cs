using System.Windows.Media;

namespace DACDataVisualization
{
    public class DataPointPreferences : IPlotPreferences
    {
        public SolidColorBrush Brush { get; set; }
        public double StrokeThickness { get; set; }
        public double XDim { get; set; }
        public double YDim { get; set; }

        private DataPointPreferences() { }

        public static DataPointPreferences CreateObject(Color c, double t, double xDim, double yDim)
        {
            DataPointPreferences dpp = new DataPointPreferences
            {
                Brush = new SolidColorBrush
                {
                    Color = c
                },
                StrokeThickness = t,
                XDim = xDim,
                YDim = yDim
            };

            return dpp;
        }
    }
}
