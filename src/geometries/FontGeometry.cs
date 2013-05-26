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
        /// The font height to draw with
        /// </summary>
        public float Height = 1;

        /// <summary>
        /// The <see cref="Font"/> to draw with, must always be set to a valid instance when this geometry is used.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface to use for drawing</param>
        /// <param name="font">The <see cref="Font"/> used</param>
        public FontGeometry(QuadSurface<UVColorVertexData> surface, Font font)
            : base(surface)
        {
            this.Font = font;
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
            if (this.Font.Monospaced)
                w = l;
            else
            {
                w = 0;
                for (int i = 0; i < l; i++)
                    w += this.Font.LetterWidth((int)s[i]);
            }
            if (accountForFontHeight)
                w *= this.Height;
            if (accountForSymbolWidth)
                w *= this.Font.SymbolSize.X;
            return w;
        }


        #region DrawString /// @name DrawString

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
                position.Y -= this.Height * alignY;
            if (alignX != 0)
                position.X -= this.StringWidth(text) * alignX;
            this.drawStringReal(position, text);
        }

        #endregion


        #region DrawMultiLineString /// @name DrawMultiLineString

        /// <summary>
        /// Draws a string, split into multiple lines by the \n character.
        /// </summary>
        /// <param name="position">The position to draw the first line at.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="alignX">The horizontal alignment. 0 for left align, 1 for right align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        /// <param name="alignY">The vertical alignment. 0 for top align, 1 for bottom align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        public void DrawMultiLineString(Vector2 position, string text, float alignX = 0, float alignY = 0)
        {
            this.DrawMultiLineString(new Vector3(position.X, position.Y, 0), text, alignX, alignY);
        }

        /// <summary>
        /// Draws a string, split into multiple lines by the \n character.
        /// </summary>
        /// <param name="position">The position to draw the first line at.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="alignX">The horizontal alignment for each line. 0 for left align, 1 for right align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        /// <param name="alignY">The vertical alignment of the entire text. 0 for top align, 1 for bottom align, other values(including values outside [0, 1]) are linearly interpolated.</param>
        public void DrawMultiLineString(Vector3 position, string text, float alignX = 0, float alignY = 0)
        {
            string[] lines = text.Split('\n');
            int l = lines.Length;
            position.Y -= this.Height * l * alignY;
            for (int i = 0; i < l; i++)
            {
                position.Y += this.Height;
                this.DrawString(position, lines[i], alignX);
            }
        }

        #endregion


        #region Actual string drawing

        /// <summary>
        /// Draws a string.
        /// </summary>
        /// <param name="position">The position to draw at.</param>
        /// <param name="text">The string to draw.</param>
        public void drawStringReal(Vector3 position, string text)
        {
            int l = text.Length;
            UVColorVertexData[] vertices = new UVColorVertexData[l * 4];

            int v_i = 0;

            Vector2 charSize = this.Font.SymbolSize * this.Height;
            Vector2 uvSymbolSize = this.Font.UVSymbolSize;
            Vector2 uvOffset = this.Font.UVSymbolOffset;
            bool monospaced = this.Font.Monospaced;

            for (int i = 0; i < l; i++)
            {
                byte c = (byte)text[i];
                float u = (c % 16) * uvSymbolSize.X + uvOffset.X;
                float v = (c / 16) * uvSymbolSize.Y + uvOffset.Y;

                float w;
                float wu;
                if (monospaced)
                {
                    w = charSize.X;
                    wu = uvSymbolSize.X;
                }
                else
                {
                    float f = this.Font.LetterWidth(c);
                    w = charSize.X * f;
                    wu = uvSymbolSize.X * f;
                }

                // left top
                vertices[v_i++] = new UVColorVertexData(position.X, position.Y, position.Z,
                    u, v, this.Color);

                // right top
                vertices[v_i++] = new UVColorVertexData(position.X + w, position.Y, position.Z,
                    u + wu, v, this.Color);

                // right bottom
                vertices[v_i++] = new UVColorVertexData(position.X + w, position.Y + charSize.Y, position.Z,
                    u + wu, v + uvSymbolSize.Y, this.Color);

                // left bottom
                vertices[v_i++] = new UVColorVertexData(position.X, position.Y + charSize.Y, position.Z,
                    u, v + uvSymbolSize.Y, this.Color);

                position.X += w;
            }

            this.Surface.AddVertices(vertices);
        }

        #endregion

    }
}
