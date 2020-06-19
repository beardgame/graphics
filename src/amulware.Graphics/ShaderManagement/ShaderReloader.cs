using System;
using OpenToolkit.Graphics.OpenGL;

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
                    return FragmentShader.FromCode(source);
                case ShaderType.VertexShader:
                    return VertexShader.FromCode(source);
                case ShaderType.GeometryShader:
                    return GeometryShader.FromCode(source);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract string GetSource();
    }
}
