using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class FloatUniform : SurfaceSetting
    {
        private string name;

        public float Float;

        public FloatUniform(string name)
            : this(name, 0) { }

        public FloatUniform(string name, float f)
        {
            this.name = name;
            this.Float = f;
        }


        public override void Set(ShaderProgram program)
        {
            GL.Uniform1(program.GetUniformLocation(this.name), this.Float);
        }
    }
}
