using OpenTK.Mathematics;

namespace Bearded.Graphics.Text
{
    // ReSharper disable ParameterHidesMember
    public sealed class TextDrawerWithDefaults<TVertexParameters>
    {
        private readonly ITextDrawer<TVertexParameters> drawer;
        private readonly float fontHeight;
        private readonly float alignHorizontal;
        private readonly float alignVertical;
        private readonly Vector3 unitRightDP;
        private readonly Vector3 unitDownDP;
        private readonly TVertexParameters parameters;

        public TextDrawerWithDefaults(ITextDrawer<TVertexParameters> drawer,
            float fontHeight, float alignHorizontal, float alignVertical,
            Vector3 unitRightDP, Vector3 unitDownDP, TVertexParameters parameters
            )
        {
            this.drawer = drawer;
            this.fontHeight = fontHeight;
            this.alignHorizontal = alignHorizontal;
            this.alignVertical = alignVertical;
            this.unitRightDP = unitRightDP;
            this.unitDownDP = unitDownDP;
            this.parameters = parameters;
        }

        public TextDrawerWithDefaults<TVertexParameters> With(
            float? fontHeight = null, float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDP = null, Vector3? unitDownDP = null)
        {
            return With(parameters, fontHeight, alignHorizontal, alignVertical, unitRightDP, unitDownDP);
        }

        public TextDrawerWithDefaults<TVertexParameters> With(TVertexParameters parameters,
            float? fontHeight = null,
            float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDP = null, Vector3? unitDownDP = null)
        {
            return new TextDrawerWithDefaults<TVertexParameters>(
                drawer, fontHeight ?? this.fontHeight,
                alignHorizontal ?? this.alignHorizontal, alignVertical ?? this.alignVertical,
                unitRightDP ?? this.unitRightDP, unitDownDP ?? this.unitDownDP, parameters
                );
        }

        public void DrawLine(
            Vector3 xyz, string text, float? fontHeight = null,
            float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDP = null, Vector3? unitDownDP = null)
        {
            DrawLine(parameters, xyz, text, fontHeight, alignHorizontal, alignVertical, unitRightDP, unitDownDP);
        }

        public void DrawLine(TVertexParameters parameters,
            Vector3 xyz, string text, float? fontHeight = null,
            float? alignHorizontal = null, float? alignVertical = null,
            Vector3? unitRightDP = null, Vector3? unitDownDP = null)
        {
            drawer.DrawLine(xyz, text, fontHeight ?? this.fontHeight,
                alignHorizontal ?? this.alignHorizontal, alignVertical ?? this.alignVertical,
                unitRightDP ?? this.unitRightDP, unitDownDP ?? this.unitDownDP, parameters);
        }

        public (Vector3 Width, Vector3 Height) StringSize(
            string text, float? fontHeight = null, Vector3? unitRightDP = null, Vector3? unitDownDP = null)
        {
            return drawer.StringSize(
                text, fontHeight ?? this.fontHeight, unitRightDP ?? this.unitRightDP, unitDownDP ?? this.unitDownDP);
        }

        public Vector3 StringWidth(string text, float? fontHeight = null, Vector3? unitRightDP = null)
        {
            return drawer.StringWidth(text, fontHeight ?? this.fontHeight, unitRightDP ?? this.unitRightDP);
        }

        public Vector3 StringHeight(float? fontHeight = null, Vector3? unitDownDP = null)
        {
            return drawer.StringHeight(fontHeight ?? this.fontHeight, unitDownDP ?? this.unitDownDP);
        }
    }
}
