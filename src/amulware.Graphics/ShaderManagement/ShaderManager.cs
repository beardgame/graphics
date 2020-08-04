using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using OpenToolkit.Graphics.OpenGL;
using static OpenToolkit.Graphics.OpenGL.ShaderType;

namespace amulware.Graphics.ShaderManagement
{
    using ReloadableShaderByName = Dictionary<string, ReloadableShader>;

    public sealed partial class ShaderManager
    {
        private readonly Dictionary<IShaderProvider, string> shaderNames
            = new Dictionary<IShaderProvider, string>();

        private readonly ImmutableDictionary<ShaderType, ReloadableShaderByName> shaders
            = new[] { ComputeShader, FragmentShader, GeometryShader, VertexShader, TessControlShader, TessEvaluationShader }
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

        public void Add(ReloadableRendererShader rendererShader, string name)
        {
            if (programs.ContainsKey(name))
               throw new ArgumentException($"Tried adding shader program with name '{name} which is already taken.");

            throwIfAnyShadersAreNotKnown(rendererShader, name);

            programs.Add(name, rendererShader);

            registerProgramForItsShaders(rendererShader);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void throwIfAnyShadersAreNotKnown(ReloadableRendererShader rendererShader, string name)
        {
            if (rendererShader.Shaders.FirstOrDefault(s => !shaderNames.ContainsKey(s)) is { } shader)
                throw new InvalidOperationException(
                    $"Tried adding shader program with unknown {shader.Shader.Type} under name '{name}." +
                    " All shaders must be added first.");
        }

        private void registerProgramForItsShaders(ReloadableRendererShader rendererShader)
        {
            foreach (var shader in rendererShader.Shaders)
            {
                programsByShader.TryGetValue(shader, out var programs);
                if (programs == null)
                {
                    programs = new List<ReloadableRendererShader>();
                    programsByShader.Add(shader, programs);
                }

                programs.Add(rendererShader);
            }
        }

        public void Add(IEnumerable<ShaderFile> shaderFiles)
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

        private ReloadableShader getShader(ShaderType type, string shaderName)
        {
            shaders[type].TryGetValue(shaderName, out var shader);
            return shader;
        }
    }
}
