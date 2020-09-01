using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public sealed class OrientedTextDrawer
    {
        private readonly ITextDrawer drawer;
        private readonly Vector3 unitRightDp;
        private readonly Vector3 unitDownDp;

        public OrientedTextDrawer(ITextDrawer drawer, Vector3 unitRightDp, Vector3 unitDownDp)
        {
            this.drawer = drawer;
            this.unitRightDp = unitRightDp;
            this.unitDownDp = unitDownDp;
        }

        public void DrawLine(Vector3 xyz, string text, float fontHeight,
            float alignHorizontal = 0, float alignVertical = 0)
        {
            drawer.DrawLine(xyz, text, fontHeight, alignHorizontal, alignVertical, unitRightDp, unitDownDp);
        }
    }
}
