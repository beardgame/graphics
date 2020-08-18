using amulware.Graphics.Shading;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public interface IShaderReloader
    {
        ShaderType Type { get; }
        bool ChangedSinceLastLoad { get; }
        Shader Load();
    }
}
