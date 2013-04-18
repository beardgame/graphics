using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class FragmentShader : Shader
    {
        public FragmentShader(string filename) : base(ShaderType.FragmentShader, filename) { }
    }
}
