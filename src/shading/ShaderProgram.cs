using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class ShaderProgram
    {
        VertexShader vertexShader;
        FragmentShader fragmentShader;

        public readonly int Handle;

        Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
        Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
            : this(new VertexShader(vertexShaderPath), new FragmentShader(fragmentShaderPath)) { }

        public ShaderProgram(VertexShader vs, FragmentShader fs)
        {
            this.vertexShader = vs;
            this.fragmentShader = fs;

            this.Handle = GL.CreateProgram();

            GL.AttachShader(this, vs);
            GL.AttachShader(this, fs);
            GL.LinkProgram(this);

            // throw exception if linking failed
            string info;
            int status_code;
            GL.GetProgramInfoLog(this, out info);
            GL.GetProgram(this, ProgramParameter.LinkStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            //Console.WriteLine("created shader program");

        }

        public void SetVertexAttributes(VertexAttribute[] vertexAttributes)
        {
            for (int i = 0; i < vertexAttributes.Length; i++)
                vertexAttributes[i].setAttribute(this);
        }

        public int GetAttributeLocation(string name)
        {
            int i;
            if (!this.attributeLocations.TryGetValue(name, out i))
            {
                i = GL.GetAttribLocation(this, name);
                this.attributeLocations.Add(name, i);
            }
            return i;
        }

        public int GetUniformLocation(string name)
        {
            int i;
            if (!this.uniformLocations.TryGetValue(name, out i))
            {
                i = GL.GetUniformLocation(this, name);
                this.uniformLocations.Add(name, i);
            }
            return i;
        }


        static public implicit operator int(ShaderProgram program)
        {
            return program.Handle;
        }

    }
}
