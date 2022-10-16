using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Bearded.Graphics
{
    /// <summary>
    /// A struct representing a 32bit argb colour.
    /// </summary>
    /// <remarks>
    /// The actual order of the components in the struct is RGBA (one byte each), to conform to how shader languages
    /// order colour components.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct Color : IEquatable<Color>
    {
        private readonly byte r, g, b, a;

        /// <summary>
        /// Constructs a colour from a red, green, blue and alpha value.
        /// </summary>
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// Constructs a colour from a 32bit unsigned integer including the colour components in the order ARGB.
        /// </summary>
        public Color(uint argb)
        {
            a = (byte) ((argb >> 24) & 255);
            r = (byte) ((argb >> 16) & 255);
            g = (byte) ((argb >> 8) & 255);
            b = (byte) (argb & 255);
        }

        /// <summary>
        /// Creates a colour from hue, saturation and value.
        /// </summary>
        /// <param name="hue">Hue of the colour (0-2pi).</param>
        /// <param name="saturation">Saturation of the colour (0-1).</param>
        /// <param name="value">Value of the colour (0-1).</param>
        /// <param name="alpha">Alpha of the colour.</param>
        public static Color FromHSVA(float hue, float saturation, float value, byte alpha = 255)
        {
            var chroma = value * saturation;
            hue /= MathHelper.PiOver3;
            var x = chroma * (1 - Math.Abs((hue % 2) - 1));
            var m = value - chroma;

            var (r, g, b) = (int) hue switch
            {
                0 => (chroma, x, 0),
                1 => (x, chroma, 0),
                2 => (0, chroma, x),
                3 => (0, x, chroma),
                4 => (x, 0, chroma),
                5 => (chroma, 0, x),
                _ => (0f, 0f, 0f),
            };

            return new Color((byte) ((r + m) * 255), (byte) ((g + m) * 255), (byte) ((b + m) * 255), alpha);
        }

        /// <summary>
        /// Creates a new gray color.
        /// </summary>
        /// <param name="value">The value (brightness) of the color.</param>
        /// <param name="alpha">The opacity of the color.</param>
        /// <returns>A gray colour with the given value and transparency.</returns>
        public static Color GrayScale(byte value, byte alpha = 255)
        {
            return new Color(value, value, value, alpha);
        }

        /// <summary>
        /// Linearly interpolates between two colours and returns the result.
        /// </summary>
        /// <param name="color0">The first colour.</param>
        /// <param name="color1">The second colour.</param>
        /// <param name="p">Interpolation parameter, is clamped to [0, 1]</param>
        /// <returns>The interpolated colour</returns>
        public static Color Lerp(Color color0, Color color1, float p)
        {
            if (p <= 0)
                return color0;
            if (p >= 1)
                return color1;

            var q = 1 - p;
            return new Color(
                (byte) (color0.r * q + color1.r * p),
                (byte) (color0.g * q + color1.g * p),
                (byte) (color0.b * q + color1.b * p),
                (byte) (color0.a * q + color1.a * p)
            );
        }

        // ReSharper disable ConvertToAutoPropertyWhenPossible
        // Disable converting to auto-properties because we want to ensure the correct sequential layout of this struct.

        /// <summary>
        /// The colour's red value
        /// </summary>
        public byte R => r;

        /// <summary>
        /// The colour's green value
        /// </summary>
        public byte G => g;

        /// <summary>
        /// The colour's blue value
        /// </summary>
        public byte B => b;

        /// <summary>
        /// The colour's alpha value (0 = fully transparent, 255 = fully opaque)
        /// </summary>
        public byte A => a;

        // ReSharper restore ConvertToAutoPropertyWhenPossible

        /// <summary>
        /// The colour, represented as 32bit unsigned integer, with its colour components in the order ARGB.
        /// </summary>
        public uint ARGB =>
            ((uint) a << 24)
            | ((uint) r << 16)
            | ((uint) g << 8)
            | b;

        /// <summary>
        /// Returns the value (lightness) of the colour in the range 0 to 1.
        /// </summary>
        public float Value => Math.Max(r, Math.Max(g, b)) * (1 / 255f);

        /// <summary>
        /// Returns the saturation of the colour in the range 0 to 1.
        /// </summary>
        public float Saturation
        {
            get
            {
                var max = Math.Max(r, Math.Max(g, b));
                if (max == 0)
                    return 0;

                var min = Math.Min(r, Math.Min(g, b));

                return (float) (max - min) / max;
            }
        }

        /// <summary>
        /// Returns the hue of the colour in the range 0 to 2pi.
        /// </summary>
        public float Hue
        {
            get
            {
                var floatR = r / 255f;
                var floatG = g / 255f;
                var floatB = b / 255f;

                float h;

                // ReSharper disable CompareOfFloatsByEqualityOperator
                var max = Math.Max(floatR, Math.Max(floatG, floatB));
                if (max == 0)
                    return 0;

                var min = Math.Min(floatR, Math.Min(floatG, floatB));
                var delta = max - min;

                if (floatR == max)
                    h = (floatG - floatB) / delta;
                else if (floatG == max)
                    h = 2 + (floatB - floatR) / delta;
                else
                    h = 4 + (floatR - floatG) / delta;
                // ReSharper restore CompareOfFloatsByEqualityOperator

                h *= (float) (Math.PI / 3);
                if (h < 0)
                    h += (float) (Math.PI * 2);

                return h;
            }
        }

        /// <summary>
        /// Converts the colour to a float vector with components Hue, Saturation, Value in that order.
        /// The range of the hue is 0 to 2pi, the range of all over components is 0 to 1.
        /// </summary>
        public Vector4 AsHSVAVector
        {
            get
            {
                var floatR = r / 255f;
                var floatG = g / 255f;
                var floatB = b / 255f;
                var floatA = a / 255f;

                // ReSharper disable CompareOfFloatsByEqualityOperator
                var max = Math.Max(floatR, Math.Max(floatG, floatB));
                if (max == 0)
                    return new Vector4(0, 0, 0, floatA);

                var v = max;
                var min = Math.Min(floatR, Math.Min(floatG, floatB));
                var delta = max - min;

                var s = delta / max;

                float h;

                if (floatR == max)
                    h = (floatG - floatB) / delta;
                else if (floatG == max)
                    h = 2 + (floatB - floatR) / delta;
                else
                    h = 4 + (floatR - floatG) / delta;
                // ReSharper restore CompareOfFloatsByEqualityOperator

                h *= (float) (Math.PI / 3);
                if (h < 0)
                    h += (float) (Math.PI * 2);

                return new Vector4(h, s, v, floatA);
            }
        }

        /// <summary>
        /// Converts the colour to a float vector with components RGBA in that order.
        /// The range of each component is 0 to 1.
        /// </summary>
        public Vector4 AsRGBAVector => new(r / 255f, g / 255f, b / 255f, a / 255f);

        /// <summary>
        /// The colour, pre-multiplied with its alpha value.
        /// </summary>
        public Color Premultiplied
        {
            get
            {
                var floatA = a * (1 / 255f);
                return new Color(
                    (byte) (r * floatA),
                    (byte) (g * floatA),
                    (byte) (b * floatA),
                    a);
            }
        }

        /// <summary>
        /// Returns a new colour with the same RGB values, but a different alpha value.
        /// </summary>
        public Color WithAlpha(byte alpha = 0) => new(r, g, b, alpha);

        /// <summary>
        /// Returns a new colour with the same RGB values, but a different alpha value.
        /// </summary>
        /// <param name="alpha">The new alpha value (0-1).</param>
        /// <remarks>
        /// This expects alpha values in the range 0 to 1.
        /// Values outside that range will result in overflow of the valid range and may lead to undesirable values.
        /// </remarks>
        public Color WithAlpha(float alpha) => WithAlpha((byte) (255 * alpha));

        public bool Equals(Color other) => r == other.r && g == other.g && b == other.b && a == other.a;

        public override bool Equals(object? obj) => obj is Color color && Equals(color);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = r.GetHashCode();
                hashCode = (hashCode * 397) ^ g.GetHashCode();
                hashCode = (hashCode * 397) ^ b.GetHashCode();
                hashCode = (hashCode * 397) ^ a.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a hexadecimal <see cref="System.String" /> that represents this color.
        /// </summary>
        public override string ToString() => "#" + ARGB.ToString("X8");

        public static implicit operator System.Drawing.Color(Color color) =>
            System.Drawing.Color.FromArgb(color.a, color.r, color.g, color.b);

        public static bool operator ==(Color color1, Color color2) => color1.Equals(color2);

        public static bool operator !=(Color color1, Color color2) => !(color1 == color2);

        /// <summary>
        /// Multiplies all components of the colour by a given scalar.
        /// Note that scalar values outside the range of 0 to 1 may result in overflow and cause unexpected results.
        /// </summary>
        public static Color operator *(Color color, float scalar) =>
            new((byte) (color.r * scalar),
                (byte) (color.g * scalar),
                (byte) (color.b * scalar),
                (byte) (color.a * scalar));

        /// <summary>
        /// Multiplies all components of the colour by a given scalar.
        /// Note that scalar values outside the range of 0 to 1 may result in overflow and cause unexpected results.
        /// </summary>
        public static Color operator *(float scalar, Color color) => color * scalar;
    }
}
