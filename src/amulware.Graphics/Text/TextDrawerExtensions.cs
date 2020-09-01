using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public static class TextDrawerExtensions
    {
        public static OrientedTextDrawer WithUnits(this ITextDrawer drawer, Vector3 unitRightDp, Vector3 unitDownDp)
        {
            return new OrientedTextDrawer(drawer, unitRightDp, unitDownDp);
        }
    }
}
