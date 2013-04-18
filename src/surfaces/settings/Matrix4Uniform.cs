using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class Matrix4Uniform : SurfaceSetting
    {
        private string name;

        public Matrix4 Matrix;


        public Matrix4Uniform(string name)
            : this(name, Matrix4.Identity) { }

        public Matrix4Uniform(string name, Matrix4 matrix)
        {
            this.name = name;
            this.Matrix = matrix;
        }

        override public void Set(ShaderProgram program)
        {
            GL.UniformMatrix4(program.GetUniformLocation(this.name), false, ref this.Matrix);
        }
    }
}
