using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Content
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public readonly struct ColorArgb : IEquatable<ColorArgb>
    {
        [FieldOffset(0)]
        private readonly byte a;
        [FieldOffset(1)]
        private readonly byte r;
        [FieldOffset(2)]
        private readonly byte g;
        [FieldOffset(3)]
        private readonly byte b;

        [FieldOffset(0)]
        private readonly uint argb;

        public byte A => a;
        public byte R => r;
        public byte G => g;
        public byte B => b;
        public uint Argb => argb;

        public static ColorArgb From(Color color) => new ColorArgb(color.ARGB);
        public static ColorArgb From(Color4 color) => new ColorArgb((uint)color.ToArgb());
        public static ColorArgb From(System.Drawing.Color color) => new ColorArgb((uint)color.ToArgb());
        public static ColorArgb FromArgb(uint argb) => new ColorArgb(argb);

        public ColorArgb(byte a, byte r, byte g, byte b) : this()
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        private ColorArgb(uint argb) : this()
        {
            this.argb = argb;
        }

        public Color4 ToColor4() => new Color4(R, G, B, A);
        public Color ToRgba() => new Color(R, G, B, A);

        public bool Equals(ColorArgb other) => argb == other.argb;
        public override bool Equals(object? obj) => obj is ColorArgb other && Equals(other);
        public override int GetHashCode() => (int)argb;
        public static bool operator ==(ColorArgb left, ColorArgb right) => left.Equals(right);
        public static bool operator !=(ColorArgb left, ColorArgb right) => !left.Equals(right);

        public override string ToString() => "#" + argb.ToString("X8");
    }
}
