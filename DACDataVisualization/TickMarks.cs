using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DACDataVisualization
{
    class TickMarks
    {
        internal List<Line> TickMarkSet { get; set; }
        internal List<TextBlock> TickMarkLabelSet { get; set; }

        private TickMarks() { }

        public static TickMarks NewGridLines(CurvePreferences cp, double startX, double endX, double startY, double endY, double numXLines, double numYLines)
        {
            TickMarks tm = new TickMarks
            {
                TickMarkSet = new List<Line>()
            };

            double xInterval = (endX - startX) / numXLines;
            for (double lineloc = startX; lineloc <= endX; lineloc += xInterval)
                tm.TickMarkSet.Add(new Line()
                {
                    X1 = lineloc,
                    X2 = lineloc,
                    Y1 = startY,
                    Y2 = endY,
                    Stroke = cp.StrokeBrush,
                    StrokeThickness = cp.Thickness
                });
            double yInterval = (endY - startY) / numYLines;
            for (double lineloc = startY; lineloc <= endY; lineloc += yInterval)
                tm.TickMarkSet.Add(new Line()
                {
                    X1 = startX,
                    X2 = endX,
                    Y1 = lineloc,
                    Y2 = lineloc,
                    Stroke = cp.StrokeBrush,
                    StrokeThickness = cp.Thickness
                });

            return tm;
        }
    }
}
