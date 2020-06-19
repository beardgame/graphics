using System.Collections.Generic;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    sealed public class Chart2D
    {
        public IAxis Axis1 { get; set; }
        public IAxis Axis2 { get; set; }

        public Grid2D Grid { get; set; }

        public List<IChart2DComponent> Data { get; set; }

        public Chart2DSpriteContainer Sprites { get; set; }

        public Vector2 Offset { get; set; }

        public Chart2D()
        {
            this.Data = new List<IChart2DComponent>();
        }


        public void Draw()
        {
            foreach (var data in this.Data)
                data.Draw(this.Sprites, this.Axis1, this.Axis2, this.Offset);
            Axis1.Draw(this.Sprites, Vector2.UnitX, this.Offset);
            Axis2.Draw(this.Sprites, Vector2.UnitY, this.Offset);
            if(this.Grid != null)
                this.Grid.Draw(this.Sprites, this.Axis1, this.Axis2, this.Offset);
        }
    }
}
