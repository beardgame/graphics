using System;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public abstract class ShaderReloader : IShaderReloader
    {
        public ShaderType Type { get; }

        public abstract bool ChangedSinceLastLoad { get; }

        protected ShaderReloader(ShaderType type)
        {
            Type = type;
        }

        public Shader Load()
        {
            var source = GetSource();

            switch (Type)
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

        protected abstract string GetSource();
    }
}
