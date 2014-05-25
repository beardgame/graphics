using System.Collections.Generic;
using OpenTK;

namespace amulware.Graphics.Charts
{
    sealed public class PointCloud2D<TPoint> : IChart2DData
    {
        private readonly List<Point2D<TPoint>> points;
        private readonly float pointSize;
        private readonly Color color;

        public PointCloud2D(List<Point2D<TPoint>> points, float pointSize, Color color)
        {
            this.points = points;
            this.pointSize = pointSize;
            this.color = color;
        }

        public void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset)
        {
            sprites.Color = this.color;
            foreach (var point in this.points)
            {
                var x = (float)axis1.DataToChart(point.Position.X);
                var y = (float)axis2.DataToChart(point.Position.Y);
                sprites.DrawPoint(new Vector2(x, y) + offset, this.pointSize);
            }
        }
    }
}
