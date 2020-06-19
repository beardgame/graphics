using System.Collections.Generic;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    sealed public class Grid2D
    {
        public Grid2D()
        {
            this.HorizontalLines = new List<GridLine>();
            this.VerticalLines = new List<GridLine>();
        }

        public Grid2D(List<GridLine> horizontalLines, List<GridLine> verticalLines)
        {
            this.HorizontalLines = horizontalLines;
            this.VerticalLines = verticalLines;
        }

        public List<GridLine> HorizontalLines { get; set; }
        public List<GridLine> VerticalLines { get; set; }

        public void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset)
        {
            if (this.HorizontalLines != null)
                foreach (var line in this.HorizontalLines)
                    line.Draw(sprites, axis1, axis2, offset, Vector2.UnitX, Vector2.UnitY);

            if (this.VerticalLines != null)
                foreach (var line in this.VerticalLines)
                    line.Draw(sprites, axis2, axis1, offset, Vector2.UnitY, Vector2.UnitX);
        }
    }
}
