using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// A struct representing a 32bit argb colour
    /// </summary>
    /// <remarks>The actual order of the components in the struct is RGBA(one byte each), to conform to how shader languages order colour components.</remarks>
    public struct Color
    {

        #region Properties

        /// <summary>
        /// The colour's red value
        /// </summary>
        public byte R;
        /// <summary>
        /// The colour's green value
        /// </summary>
        public byte G;
        /// <summary>
        /// The colour's blue value
        /// </summary>
        public byte B;
        /// <summary>
        /// The colour's alpha value (0 = fully transparent, 255 = fully opaque)
        /// </summary>
        public byte A;

        #endregion

        #region Static Colours

        // primary
        public static readonly Color White      = new Color(0xFFFFFFFF);
        public static readonly Color Silver     = new Color(0xFFC0C0C0);
        public static readonly Color Gray       = new Color(0xFF808080);
        public static readonly Color Black      = new Color(0xFF000000);
        public static readonly Color Red        = new Color(0xFFFF0000);
        public static readonly Color Maroon     = new Color(0xFF800000);
        public static readonly Color Yellow     = new Color(0xFFFFFF00);
        public static readonly Color Olive      = new Color(0xFF808000);
        public static readonly Color Lime       = new Color(0xFF00FF00);
        public static readonly Color Green      = new Color(0xFF008000);
        public static readonly Color Aqua       = new Color(0xFF00FFFF);
        public static readonly Color Teal       = new Color(0xFF008080);
        public static readonly Color Blue       = new Color(0xFF0000FF);
        public static readonly Color Navy       = new Color(0xFF000080);
        public static readonly Color Fuchsia    = new Color(0xFFFF00FF);
        public static readonly Color Purple     = new Color(0xFF800080);

        //secondary
        public static readonly Color Orange = new Color(0xFFFFA500);
        public static readonly Color DimGray = new Color(0xFF696969);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a fully opaque colour from a red, green and blue value
        /// </summary>
        /// <param name="r">the red value</param>
        /// <param name="g">the green value</param>
        /// <param name="b">the blue value</param>
        public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

        /// <summary>
        /// Constructs a colour from a red, green, blue and alpha value
        /// </summary>
        /// <param name="r">the red value</param>
        /// <param name="g">the green value</param>
        /// <param name="b">the blue value</param>
        /// <param name="a">the alpha value</param>
        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Constructs a colour from a 32bit unsigned integer including the colour components in the order ARGB
        /// </summary>
        /// <param name="argb">the unsigned integer representing the colour</param>
        public Color(uint argb)
        {
            this.A = (byte)((argb >> 24) & 255);
            this.R = (byte)((argb >> 16) & 255);
            this.G = (byte)((argb >> 8) & 255);
            this.B = (byte)(argb & 255);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a colour from hue, saturation and value
        /// </summary>
        /// <param name="h">hue of the colour (0-2pi)</param>
        /// <param name="s">saturation of the colour (0-1)</param>
        /// <param name="v">value of the colour (0-1)</param>
        /// <param name="a">alpha of the colour</param>
        /// <returns>the constructed colour</returns>
        public static Color FromHSVA(float h, float s, float v, byte a = 1)
        {
            float chroma = v * s;
	        h /= MathHelper.PiOver3;
	        float x = chroma * (1 - Math.Abs((h % 2) - 1));
	        float m = v - chroma;
	        float r, g, b;
	        if(h > 6 || h < 0)
            {
		        r = 0;
		        g = 0;
		        b = 0;
            }
	        else if(h < 1)
            {
		        r = chroma;
		        g = x;
		        b = 0;
            }
	        else if(h < 2)
            {
		        r = x;
		        g = chroma;
		        b = 0;
            }
	        else if(h < 3)
            {
		        r = 0;
		        g = chroma;
		        b = x;
            }
	        else if(h < 4)
            {
		        r = 0;
		        g = x;
		        b = chroma;
            }
	        else if(h < 5)
            {
		        r = x;
		        g = 0;
		        b = chroma;
            }
	        else
            {
		        r = chroma;
		        g = 0;
		        b = x;
            }
            return new Color((byte)((r + m) * 255), (byte)((g + m) * 255), (byte)((b + m) * 255), a);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The colour, represented as 32bit unsigned integer, with its colour components in the order ARGB
        /// </summary>
        public uint ARGB
        {
            get
            {
                return ((uint)this.A << 24)
                    | ((uint)this.R << 16)
                    | ((uint)this.G << 8)
                    | ((uint)this.B);
            }

            set
            {
                this = new Color(value);
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "#" + this.ARGB.ToString("X8");
        }

        #endregion


        #region Operators

        static public implicit operator System.Drawing.Color(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion

    }
}
