using OpenToolkit.Mathematics;

namespace amulware.Graphics.Text
{
    public sealed class TextDrawerWithDefaults<TVertexParameters>
    {
        private readonly ITextDrawer<TVertexParameters> drawer;
        private readonly float fontHeight;
        private readonly float alignHorizontal;
        private readonly float alignVertical;
        private readonly Vector3 unitRightDp;
        private readonly Vector3 unitDownDp;
        private readonly TVertexParameters parameters;

        public TextDrawerWithDefaults(ITextDrawer<TVertexParameters> drawer,
            float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDp, Vector3 unitDownDp, TVertexParameters parameters
            )
        {
            this.drawer = drawer;
            this.fontHeight = fontHeight;
            this.alignHorizontal = alignHorizontal;
            this.alignVertical = alignVertical;
            this.unitRightDp = unitRightDp;
            this.unitDownDp = unitDownDp;
            this.parameters = parameters;
        }

        public TextDrawerWithDefaults<TVertexParameters> With(
            float? fontHeight = null, float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDp = null, Vector3? unitDownDp = null)
        {
            return With(parameters, fontHeight, alignHorizontal, alignVertical, unitRightDp, unitDownDp);
        }

        public TextDrawerWithDefaults<TVertexParameters> With(TVertexParameters parameters,
            float? fontHeight = null,
            float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDp = null, Vector3? unitDownDp = null)
        {
            return new TextDrawerWithDefaults<TVertexParameters>(
                drawer, fontHeight ?? this.fontHeight,
                alignHorizontal ?? this.alignHorizontal, alignVertical ?? this.alignVertical,
                unitRightDp ?? this.unitRightDp, unitDownDp ?? this.unitDownDp, parameters
                );
        }

        public void DrawLine(
            Vector3 xyz, string text, float? fontHeight = null,
            float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDp = null, Vector3? unitDownDp = null)
        {
            DrawLine(parameters, xyz, text, fontHeight, alignHorizontal, alignVertical, unitRightDp, unitDownDp);
        }

        public void DrawLine(TVertexParameters parameters,
            Vector3 xyz, string text, float? fontHeight = null,
            float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDp = null, Vector3? unitDownDp = null)
        {
            drawer.DrawLine(xyz, text, fontHeight ?? this.fontHeight,
                alignHorizontal ?? this.alignHorizontal, alignVertical ?? this.alignVertical,
                unitRightDp ?? this.unitRightDp, unitDownDp ?? this.unitDownDp, parameters);
        }
    }
}
