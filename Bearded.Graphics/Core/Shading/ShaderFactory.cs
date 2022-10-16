using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Shading
{
    public sealed class ShaderFactory
    {
        public static ShaderFactory Compute { get; } = new(ShaderType.ComputeShader);
        public static ShaderFactory Fragment { get; } = new(ShaderType.FragmentShader);
        public static ShaderFactory Geometry { get; } = new(ShaderType.GeometryShader);
        public static ShaderFactory TessellationControl { get; } = new(ShaderType.TessControlShader);
        public static ShaderFactory TessellationEvaluation { get; } = new(ShaderType.TessEvaluationShader);
        public static ShaderFactory Vertex { get; } = new(ShaderType.VertexShader);

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
