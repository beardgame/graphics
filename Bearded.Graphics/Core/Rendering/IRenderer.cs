using System;
using Bearded.Graphics.Shading;

namespace Bearded.Graphics.Rendering
{
    public interface IRenderer : IDisposable
    {
        public void SetShaderProgram(ShaderProgram program);

        public void Render();
    }
}
