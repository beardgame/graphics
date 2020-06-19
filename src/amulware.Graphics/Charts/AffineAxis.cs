using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    sealed public class AffineAxis : IAxis
    {
        private readonly double originValue;
        private readonly double scale;
        private readonly float lengthPositive;
        private readonly float lengthNegative;
        private readonly Color color;

        public AffineAxis(double originValue, double scale, float lengthPositive, float lengthNegative, Color color)
        {
            this.originValue = originValue;
            this.scale = scale;
            this.lengthPositive = lengthPositive;
            this.lengthNegative = lengthNegative;
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

        public double OriginValue
        {
            get { return this.originValue; }
        }

        public double MinValue
        {
            get { return this.ChartToData(-this.lengthNegative) - this.originValue; }
        }

        public double MaxValue
        {
            get { return this.ChartToData(this.lengthPositive) - this.originValue; }
        }

        public void Draw(Chart2DSpriteContainer sprites, Vector2 direction, Vector2 offset)
        {
            sprites.Color = this.color;
            sprites.DrawLine(offset - direction * this.lengthNegative,
                offset + direction * this.lengthPositive,
                sprites.ThinLineWidth);
        }
    }
}
