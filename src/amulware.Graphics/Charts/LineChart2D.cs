using System.Collections.Generic;
using OpenTK;

namespace amulware.Graphics.Charts
{
    public class LineChart2D<TPoint> : DiscreteFunction2D<TPoint>, IChart2DComponent
        where TPoint : IPoint2D
    {
        private readonly Color color;

        public LineChart2D(IEnumerable<TPoint> points, Color color)
            : base(points)
        {
            this.color = color;
        }

        public void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset)
        {
            sprites.Color = this.color;

            Vector2 lastPoint = default(Vector2);
            bool first = true;
            foreach (var point in this.drawablePoints)
            {
                var pd = point.Position;

                var p = offset + new Vector2((float)axis1.DataToChart(pd.X), (float)axis2.DataToChart(pd.Y));

                if (first)
                {
                    first = false;
                    lastPoint = p;
                    continue;
                }

                sprites.DrawLine(lastPoint, p, sprites.ThickLineWidth);

                lastPoint = p;
            }
        }
    }
}
