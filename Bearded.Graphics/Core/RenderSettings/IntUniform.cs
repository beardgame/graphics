using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.RenderSettings;

public sealed class IntUniform : Uniform<int>
{
    public IntUniform(string name) : base(name, 0)
    {
    }

    public IntUniform(string name, int value) : base(name, value)
    {
    }

    protected override void SetAtLocation(int location)
    {
        GL.Uniform1(location, Value);
    }
}
