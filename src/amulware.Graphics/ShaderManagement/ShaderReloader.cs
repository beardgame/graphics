using System;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    abstract class ShaderReloader : IShaderReloader
    {
        private readonly ShaderType type;
        public ShaderType Type { get { return this.type; } }

        public abstract bool ChangedSinceLastLoad { get; }

        protected ShaderReloader(ShaderType type)
        {
            this.type = type;
        }

        public Shader Load()
        {
            var source = this.getSource();

            switch (this.type)
            {
                case ShaderType.FragmentShader:
                    return new FragmentShader(source);
                case ShaderType.VertexShader:
                    return new VertexShader(source);
                case ShaderType.GeometryShader:
                    return new GeometryShader(source);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract string getSource();
    }
}
