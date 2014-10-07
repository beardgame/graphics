using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace amulware.Graphics.Charts
{
    sealed public class StackingBarChart2D : RegularDiscreteFunction2D<IList<double>>
    {
        private readonly List<Color> colors;

        public StackingBarChart2D(params Color[] colors)
            : this(colors.AsEnumerable())
        {
            
        }

        public StackingBarChart2D(IEnumerable<Color> colors)
        {
            this.colors = colors.ToList();
        }

        public override void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset)
        {
            float previousX = 0;
            bool first = true;

            float baseDrawY = (float)axis2.DataToChart(0) + offset.Y;

            foreach (var bars in this.drawnPoints(axis1, offset))
            {
                if (first)
                {
                    previousX = bars.XCoordinate;
                    first = false;
                    continue;
                }

                var y = 0f;
                var i = 0;

                var drawY = baseDrawY;

                foreach (var value in bars.Value)
                {
                    sprites.Color = this.colors[(i++) % this.colors.Count];

                    var newY = y + (float)value;

                    var newDrawY = (float)axis2.DataToChart(newY) + offset.Y;

                    sprites.DrawQuad(
                        new Vector2(previousX, drawY),
                        new Vector2(previousX, newDrawY),
                        new Vector2(bars.XCoordinate, newDrawY),
                        new Vector2(bars.XCoordinate, drawY)
                        );

                    drawY = newDrawY;
                    y = newY;
                }

                previousX = bars.XCoordinate;

            }
        }
    }
}
