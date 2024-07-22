using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.BlendingFactor;
using static OpenTK.Graphics.OpenGL.BlendEquationMode;

namespace Bearded.Graphics.Pipelines.Context;

public readonly record struct BlendMode(
    BlendingFactor Source, BlendingFactor Destination, BlendEquationMode Function)
{
    public static BlendMode Disable => default;

    public static BlendMode Alpha => new(SrcAlpha, OneMinusSrcAlpha, FuncAdd);
    public static BlendMode Add => new(SrcAlpha, One, FuncAdd);
    public static BlendMode Subtract => new(SrcAlpha, One, FuncReverseSubtract);
    public static BlendMode Multiply => new(Zero, SrcColor, FuncAdd);
    public static BlendMode Premultiplied => new(One, OneMinusSrcAlpha, FuncAdd);
    public static BlendMode Min => new(One, One, BlendEquationMode.Min);
    public static BlendMode Max => new(One, One, BlendEquationMode.Max);
}
