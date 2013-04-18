using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class Vector4Uniform : SurfaceSetting
    {
        private string name;

        public Vector4 Vector;

        public Vector4Uniform(string name)
            : this(name, Vector4.Zero) { }

        public Vector4Uniform(string name, Vector4 vector)
        {
            this.name = name;
            this.Vector = vector;
        }


        public override void Set(ShaderProgram program)
        {
            GL.Uniform4(program.GetUniformLocation(this.name), this.Vector);
        }
    }
}
