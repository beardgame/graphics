using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    interface IShaderReloader
    {
        bool ChangedSinceLastLoad { get; }
        ShaderType Type { get; }
        Shader Load();
    }
}
