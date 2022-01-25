using OpenTK.Mathematics;

namespace Bearded.Graphics.Content
{
    public static class Color4Extensions
    {
        public static Color ToColor(this Color4 color)
        {
            return new Color((uint)color.ToArgb());
        }
        public static Vector4 ToVector(this Color4 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }
    }
}
