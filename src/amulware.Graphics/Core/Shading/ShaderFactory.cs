using System.IO;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class ShaderFactory
    {
        public static ShaderFactory Compute { get; } = new ShaderFactory(ShaderType.ComputeShader);
        public static ShaderFactory Fragment { get; } = new ShaderFactory(ShaderType.FragmentShader);
        public static ShaderFactory Geometry { get; } = new ShaderFactory(ShaderType.GeometryShader);
        public static ShaderFactory TessellationControl { get; } = new ShaderFactory(ShaderType.TessControlShader);
        public static ShaderFactory TessellationEvaluation { get; } = new ShaderFactory(ShaderType.TessEvaluationShader);
        public static ShaderFactory Vertex { get; } = new ShaderFactory(ShaderType.VertexShader);

        private readonly ShaderType type;

        private ShaderFactory(ShaderType type)
        {
            this.type = type;
        }

        public Shader FromCode(string sourceCode)
        {
            return Shader.Create(type, sourceCode);
        }

        public Shader FromFile(string filename)
        {
            using var streamReader = new StreamReader(filename);
            var code = streamReader.ReadToEnd();
            return FromCode(code);
        }
    }
}