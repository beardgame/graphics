using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Geometry that draws ASCII strings
    /// </summary>
    public class FontGeometry : Geometry<UVColorVertexData>
    {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// The UV origin in the font texture
        /// </summary>
        public Vector2 UVOffset = Vector2.Zero;
        /// <summary>
        /// The size of a character in UV coordinates
        /// </summary>
        public Vector2 UVSymbolSize = new Vector2(1 / 16f, 1 / 16f);

        /// <summary>
        /// The dimensions of a symbol
        /// </summary>
        public Vector2 SymbolSize = new Vector2(1, 1);

        /// <summary>
        /// The font height to draw with
        /// </summary>
        public float Height = 1;

        private FontSetting setting;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface to use for drawing</param>
        public FontGeometry(QuadSurface<UVColorVertexData> surface)
            : base(surface)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface to use for drawing</param>
        /// <param name="setting">The <see cref="FontSetting"/> used</param>
        public FontGeometry(QuadSurface<UVColorVertexData> surface, FontSetting setting)
            : base(surface)
        {
            this.setting = setting;
        }

        /// <summary>
        /// Calculates the width of a string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="accountForFontHeight">if set to <c>true</c> multiplies result with the set font height</param>
        /// <param name="accountForSymbolWidth">if set to <c>true</c> multiplies result with the default symbol width</param>
        /// <returns>Width of the string</returns>
        public float StringWidth(string s, bool accountForFontHeight = true, bool accountForSymbolWidth = true)
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
            if (accountForSymbolWidth)
                w *= this.SymbolSize.X;
            return w;
        }


        #region DrawString /// @name DrawString

        #region 2D Overloads

        /// <summary>
        /// Draws a string.
        /// </summary>
        /// <param name="position">The position to draw at.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="alignX">The horizontal alignment. 0 for left align, 1 for right align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        /// <param name="alignY">The vertical alignment. 0 for top align, 1 for bottom align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        public void DrawString(Vector2 position, string text, float alignX = 0, float alignY = 0)
        {
            this.DrawString(new Vector3(position.X, position.Y, 0), text, alignX, alignY);
        }

        #endregion

        /// <summary>
        /// Draws a string.
        /// </summary>
        /// <param name="position">The position to draw at.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="alignX">The horizontal alignment. 0 for left align, 1 for right align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        /// <param name="alignY">The vertical alignment. 0 for top align, 1 for bottom align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        public void DrawString(Vector3 position, string text, float alignX = 0, float alignY = 0)
        {
            if (alignY != 0)
                position.Y -= this.Height * this.SymbolSize.Y * alignY;
            if (alignX != 0)
                position.X -= this.StringWidth(text) * alignX;
            this.DrawString(position, text, alignX);
        }

        #endregion



        #region Actual string drawing

        /// <summary>
        /// Draws a string.
        /// </summary>
        /// <param name="position">The position to draw at.</param>
        /// <param name="text">The string to draw.</param>
        public void drawString(Vector3 position, string text)
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

        #endregion

    }
}
