using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.ShaderManagement
{
    public interface IShaderReloader
    {
        ShaderType Type { get; }
        bool ChangedSinceLastLoad { get; }
        Shader Load();
    }
}
