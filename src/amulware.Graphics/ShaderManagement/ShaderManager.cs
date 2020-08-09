using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using OpenToolkit.Graphics.OpenGL;
using static OpenToolkit.Graphics.OpenGL.ShaderType;

namespace amulware.Graphics.ShaderManagement
{
    using ReloadableShaderByName = Dictionary<string, ReloadableShader>;

    public sealed partial class ShaderManager : IDisposable
    {
        private readonly Dictionary<IShaderProvider, string> shaderNames
            = new Dictionary<IShaderProvider, string>();

        private readonly ImmutableDictionary<ShaderType, ReloadableShaderByName> shaders
            = new[] { ComputeShader, FragmentShader, GeometryShader,
                    VertexShader, TessControlShader, TessEvaluationShader }
                .ToImmutableDictionary(k => k, _ => new ReloadableShaderByName());

        private readonly Dictionary<string, ReloadableRendererShader> programs
            = new Dictionary<string, ReloadableRendererShader>();

        private readonly Dictionary<IShaderProvider, List<ReloadableRendererShader>> programsByShader
            = new Dictionary<IShaderProvider, List<ReloadableRendererShader>>();

        public IRendererShader? this[string shaderProgramName]
        {
            get
            {
                programs.TryGetValue(shaderProgramName, out var program);
                return program;
            }
        }

        public bool Contains(ShaderType type, string name)
        {
            if (shaders.TryGetValue(type, out var shadersOfType))
                return shadersOfType.ContainsKey(name);

            throw new ArgumentException($"ShaderType {type} is not supported.");
        }

        private void registerProgramForItsShaders(ReloadableRendererShader rendererShader)
        {
            foreach (var shader in rendererShader.Shaders)
            {
                if (!programsByShader.TryGetValue(shader, out var programs))
                {
                    programs = new List<ReloadableRendererShader>();
                    programsByShader.Add(shader, programs);
                }

                programs.Add(rendererShader);
            }
        }

        public void AddRange(IEnumerable<ShaderFile> shaderFiles)
        {
            foreach (var file in shaderFiles)
            {
                Add(file);
            }
        }

        public void Add(ShaderFile shaderFile)
        {
            Add(shaderFile, shaderFile.FriendlyName);
        }

        public void Add(IShaderReloader shader, string name)
        {
            Add(ReloadableShader.LoadFrom(shader), name);
        }

        public void Add(ReloadableShader shader, string name)
        {
            if (shaderNames.ContainsKey(shader))
                throw new ArgumentException($"Shader already known by name '{shaderNames[shader]}'.");

            var shadersOfType = shaders[shader.Type];
            shadersOfType.Add(name, shader); // will throw if name already taken

            shaderNames.Add(shader, name);
        }

        public void Dispose()
        {
            foreach (var program in programs.Values)
            {
                program.Dispose();
            }

            foreach (var shader in shaders.Values.SelectMany(s => s.Values))
            {
                shader.Dispose();
            }
        }
    }
}
