using System.Windows.Media;

namespace DACDataVisualization
{
    interface IPlotPreferences
    {
        SolidColorBrush Brush { get; set; }
        double StrokeThickness { get; set; }
    }
}
