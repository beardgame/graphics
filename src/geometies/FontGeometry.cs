using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class FontGeometry : Geometry<UVColorVertexData>
    {
        public Color Color = Color.White;

        public Vector2 UVOffset = Vector2.Zero;
        public Vector2 UVSymbolSize = new Vector2(1 / 16f, 1 / 16f);

        public Vector2 SymbolSize = new Vector2(1, 1);

        public float Height = 1;

        private FontSetting setting;

        public FontGeometry(QuadSurface<UVColorVertexData> surface)
            : base(surface)
        {
        }

        public FontGeometry(QuadSurface<UVColorVertexData> surface, FontSetting setting)
            : base(surface)
        {
            this.setting = setting;
        }

        public float StringWidth(string s, bool accountForFontHeight = true, bool accoutForSymbolWidth = true)
        {
            float w;
            int l = s.Length;
            if (this.setting == null)
                w = l;
            else
            {
                w = 0;
                for (int i = 0; i < l; i++)
                    w += this.setting.Width((int)s[i]);
            }
            if (accountForFontHeight)
                w *= this.Height;
            if (accoutForSymbolWidth)
                w *= this.SymbolSize.X;
            return w;
        }

        public void DrawString(Vector3 position, string text)
        {
            int l = text.Length;
            UVColorVertexData[] vertices = new UVColorVertexData[l * 4];

            int v_i = 0;

            Vector2 charSize = this.SymbolSize * this.Height;

            for (int i = 0; i < l; i++)
            {
                byte c = (byte)text[i];
                float u = (c % 16) * this.UVSymbolSize.X + this.UVOffset.X;
                float v = (c / 16) * this.UVSymbolSize.Y + this.UVOffset.Y;

                float w;
                float wu;
                if (this.setting == null)
                {
                    w = charSize.X;
                    wu = this.UVSymbolSize.X;
                }
                else
                {
                    float f = this.setting.Width(c);
                    w = charSize.X * f;
                    wu = this.UVSymbolSize.X * f;
                }

                // left top
                vertices[v_i++] = new UVColorVertexData(position.X, position.Y, position.Z,
                    u, v, this.Color);

                // right top
                vertices[v_i++] = new UVColorVertexData(position.X + w, position.Y, position.Z,
                    u + wu, v, this.Color);

                // right bottom
                vertices[v_i++] = new UVColorVertexData(position.X + w, position.Y + charSize.Y, position.Z,
                    u + wu, v + this.UVSymbolSize.Y, this.Color);

                // left bottom
                vertices[v_i++] = new UVColorVertexData(position.X, position.Y + charSize.Y, position.Z,
                    u, v + this.UVSymbolSize.Y, this.Color);

                position.X += w;
            }

            this.Surface.AddVertices(vertices);
        }

    }
}
