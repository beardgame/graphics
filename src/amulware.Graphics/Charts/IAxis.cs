using OpenTK;

namespace amulware.Graphics.Charts
{
    public interface IAxis
    {
        double DataToChart(double x);
        double ChartToData(double x);

        void Draw(Chart2DSpriteContainer sprites, Vector2 direction, Vector2 offset);
    }
}
