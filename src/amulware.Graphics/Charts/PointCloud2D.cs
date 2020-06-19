using System.Collections.Generic;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    sealed public class PointCloud2D<TPoint> : IChart2DComponent
        where TPoint : IPoint2D
    {
        private readonly List<TPoint> points;
        private readonly Color color;

        public PointCloud2D(List<TPoint> points, Color color)
        {
            this.points = points;
            this.color = color;
        }

        public void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset)
        {
            sprites.Color = this.color;
            foreach (var point in this.points)
            {
                var x = (float)axis1.DataToChart(point.Position.X);
                var y = (float)axis2.DataToChart(point.Position.Y);
                sprites.DrawPoint(new Vector2(x, y) + offset, sprites.LargePointSize);
            }
        }
    }
}
