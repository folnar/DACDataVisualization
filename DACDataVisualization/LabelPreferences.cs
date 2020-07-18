using System.Windows;
using System.Windows.Media;

namespace DACDataVisualization
{
    public enum LabelOrientations
    {
        Normal,
        VerticalBottomToTop,
        VerticalTopToBottom
    }

    public class LabelPreferences
    {
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
        public FontFamily Font { get; set; }
        public double Fontsize { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public LabelOrientations Orientation { get; set; }

        private LabelPreferences() { }

        public static LabelPreferences NewLabelPreferences(Brush foreground, Brush background, FontFamily font, double fontSize, FontStyle fontStyle, FontWeight fontWeight, LabelOrientations labelOrientation = LabelOrientations.Normal)
        {
            return new LabelPreferences
            {
                Foreground = foreground,
                Background = background,
                Font = font,
                Fontsize = fontSize,
                Style = fontStyle,
                Weight = fontWeight,
                Orientation = labelOrientation
            };
        }
    }
}
