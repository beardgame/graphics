using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    public interface IChart2DComponent
    {
        void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset);
    }
}
