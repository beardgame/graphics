using OpenTK;

namespace amulware.Graphics.Charts
{
    sealed public class AffineAxis : IAxis
    {
        private readonly double originValue;
        private readonly double scale;
        private readonly float drawThickness;
        private readonly float length;
        private readonly Color color;

        public AffineAxis(double originValue, double scale, float drawThickness, float length, Color color)
        {
            this.originValue = originValue;
            this.scale = scale;
            this.drawThickness = drawThickness;
            this.length = length;
            this.color = color;
        }

        public double DataToChart(double x)
        {
            return (x - this.originValue) * this.scale;
        }

        public double ChartToData(double x)
        {
            return x / this.scale + this.originValue;
        }

        public void Draw(Chart2DSpriteContainer sprites, Vector2 direction, Vector2 offset)
        {
            sprites.Color = this.color;
            sprites.DrawLine(offset, offset + direction * this.length, this.drawThickness);
        }
    }
}
