using OpenTK;

namespace amulware.Graphics.Charts
{
    public class GridLine
    {
        private readonly float startValue;
        private readonly float interval;
        private readonly bool repeat;
        private readonly Color color;

        public GridLine(Color color, float startValue)
        {
            this.startValue = startValue;
            this.color = color;
        }

        public GridLine(Color color, float startValue, float interval)
        {
            this.startValue = startValue;
            this.interval = interval;
            this.color = color;
            this.repeat = true;
        }

        public void Draw(Chart2DSpriteContainer sprites, IAxis parallelAxis, IAxis perpendicularAxis,
            Vector2 offset, Vector2 direction, Vector2 axisUnit)
        {
            sprites.Color = this.color;

            var minOffset = offset + direction * (float)parallelAxis.DataToChart(parallelAxis.MinValue);
            var maxOffset = offset + direction * (float)parallelAxis.DataToChart(parallelAxis.MaxValue);

            int minI = this.repeat ? (int)((perpendicularAxis.MinValue - this.startValue) / this.interval) : 0;
            int maxI = this.repeat ? (int)((perpendicularAxis.MaxValue - this.startValue) / this.interval) : 0;

            for (int i = minI; i <= maxI; i++)
            {
                var position = axisUnit * (float)perpendicularAxis.DataToChart(this.startValue + this.interval * i);

                sprites.DrawLine(
                    minOffset + position,
                    maxOffset + position,
                    sprites.ThinLineWidth);
            }
        }
    }
}
