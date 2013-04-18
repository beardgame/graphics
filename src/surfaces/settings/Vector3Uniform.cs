using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class Vector3Uniform : SurfaceSetting
    {
        private string name;

        public Vector3 Vector;

        public Vector3Uniform(string name)
            : this(name, Vector3.Zero) { }

        public Vector3Uniform(string name, Vector3 vector)
        {
            this.name = name;
            this.Vector = vector;
        }


        public override void Set(ShaderProgram program)
        {
            GL.Uniform3(program.GetUniformLocation(this.name), this.Vector);
        }
    }
}
