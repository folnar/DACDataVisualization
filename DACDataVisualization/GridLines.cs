using System.Collections.Generic;
using System.Windows.Shapes;

namespace DACDataVisualization
{
    class GridLines
    {
        internal List<Line> GridLineSet { get; set; }

        private GridLines() { }

        public static GridLines NewGridLines(CurvePreferences cp, double startX, double endX, double startY, double endY, double numXLines, double numYLines)
        {
            GridLines gl = new GridLines
            {
                GridLineSet = new List<Line>()
            };

            double xInterval = (endX - startX) / numXLines;
            for (double lineloc = startX; lineloc <= endX; lineloc += xInterval)
                gl.GridLineSet.Add(new Line()
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
                gl.GridLineSet.Add(new Line()
                {
                    X1 = startX,
                    X2 = endX,
                    Y1 = lineloc,
                    Y2 = lineloc,
                    Stroke = cp.StrokeBrush,
                    StrokeThickness = cp.Thickness
                });

            return gl;
        }
    }
}
