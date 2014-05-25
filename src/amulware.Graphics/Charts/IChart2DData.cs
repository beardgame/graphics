using OpenTK;

namespace amulware.Graphics.Charts
{
    public interface IChart2DData
    {
        void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset);
    }
}
