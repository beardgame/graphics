using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class Vector2Uniform : SurfaceSetting
    {
        private string name;

        public Vector2 Vector;

        public Vector2Uniform(string name)
            : this(name, Vector2.Zero) { }

        public Vector2Uniform(string name, Vector2 vector)
        {
            this.name = name;
            this.Vector = vector;
        }


        public override void Set(ShaderProgram program)
        {
            GL.Uniform2(program.GetUniformLocation(this.name), this.Vector);
        }
    }
}
