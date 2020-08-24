using System.Windows.Media;

namespace DACDataVisualization
{
    interface IPlotPreferences
    {
        Brush Brush { get; set; }
        double StrokeThickness { get; set; }
    }
}
