using System.Windows;
using System.Windows.Media;

namespace DACDataVisualization
{
    public class LabelPreferences
    {
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
        public FontFamily Font { get; set; }
        public double Fontsize { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }

        private LabelPreferences() { }

        public static LabelPreferences NewLabelPreferences(Brush foreground, Brush background, FontFamily font, double fontSize, FontStyle fontStyle, FontWeight fontWeight)
        {
            return new LabelPreferences
            {
                Foreground = foreground,
                Background = background,
                Font = font,
                Fontsize = fontSize,
                Style = fontStyle,
                Weight = fontWeight
            };
        }
    }
}
