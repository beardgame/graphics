using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class VertexShader : Shader
    {
        public VertexShader(string filename) : base(ShaderType.VertexShader, filename) { }
    }
}
