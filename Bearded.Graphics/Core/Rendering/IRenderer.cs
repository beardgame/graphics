using System;
using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public interface IRenderer : IDisposable
    {
        public void SetShaderProgram(ShaderProgram program);

        public void Render();
    }
}
