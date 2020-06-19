using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public interface IShaderReloader
    {
        bool ChangedSinceLastLoad { get; }
        ShaderType Type { get; }
        Shader Load();
    }
}
