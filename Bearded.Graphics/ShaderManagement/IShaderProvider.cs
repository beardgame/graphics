using Bearded.Graphics.Shading;

namespace Bearded.Graphics.ShaderManagement
{
    public interface IShaderProvider
    {
        public Shader Shader { get; }
    }
}
