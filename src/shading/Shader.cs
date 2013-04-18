using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public abstract class Shader
    {
        public readonly int Handle;

        public Shader(ShaderType type, string filename)
        {
            this.Handle = GL.CreateShader(type);
            StreamReader streamReader = new StreamReader(filename);
            GL.ShaderSource(this, streamReader.ReadToEnd());
            streamReader.Close();
            GL.CompileShader(this);

            // throw exception if compile failed
            string info;
            int status_code;
            GL.GetShaderInfoLog(this, out info);
            GL.GetShader(this, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            //Console.WriteLine(type.ToString() + " created");
        }

        static public implicit operator int(Shader shader)
        {
            return shader.Handle;
        }
    }
}
