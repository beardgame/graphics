using System;

namespace Bearded.Graphics.Pipelines.Context
{
    [Flags]
    public enum ColorMask
    {
        DrawRed = 1,
        DrawGreen = 2,
        DrawBlue = 4,
        DrawAlpha = 8,

        DrawAll = DrawRed | DrawGreen | DrawBlue | DrawAlpha,
    }

    sealed class ColorMask<TState> : ContextChange<TState, ColorMask>
    {
        public ColorMask(ColorMask newValue) : base(newValue)
        {
        }

        protected override ColorMask GetCurrent() => GLState.ColorMask;

        protected override void Set(ColorMask value) => GLState.SetColorMask(value);
    }
}
