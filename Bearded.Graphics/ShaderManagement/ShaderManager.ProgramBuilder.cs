using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.ShaderType;

namespace Bearded.Graphics.ShaderManagement
{
    public sealed partial class ShaderManager
    {
        public IRendererShader RegisterRendererShaderFromAllShadersWithName(string shaderName, string? name = null)
            => RegisterRendererShader(b => b.TryAll(shaderName), name ?? shaderName);

        public IRendererShader RegisterRendererShader(Action<RendererShaderBuilder> build, string name)
        {
            var builder = new RendererShaderBuilder(this);
            build(builder);

            throwIfShaderProgramNameAlreadyTaken(name);

            var program = ReloadableRendererShader.LoadFrom(builder.Shaders);
            programs.Add(name, program);
            registerProgramForItsShaders(program);

            return program;
        }

        private void throwIfShaderProgramNameAlreadyTaken(string name)
        {
            if (programs.ContainsKey(name))
                throw new ArgumentException($"Tried adding shader program with name '{name} which is already taken.");
        }

        public sealed class RendererShaderBuilder
        {
            private readonly ShaderManager manager;
            private readonly List<ReloadableShader> shaders = new();

            internal IEnumerable<ReloadableShader> Shaders => shaders;

            internal RendererShaderBuilder(ShaderManager manager)
            {
                this.manager = manager;
            }

            public RendererShaderBuilder TryAll(string shaderName)
            {
                TryWith(ComputeShader, shaderName);
                TryWith(FragmentShader, shaderName);
                TryWith(GeometryShader, shaderName);
                TryWith(TessControlShader, shaderName);
                TryWith(TessEvaluationShader, shaderName);
                TryWith(VertexShader, shaderName);
                return this;
            }

            public RendererShaderBuilder With(ShaderType type, string shaderName)
            {
                TryWith(type, shaderName, out var succeeded);
                if (!succeeded)
                    throw new ArgumentException($"{type} with name '{shaderName}' not found.");
                return this;
            }

            public RendererShaderBuilder TryWith(ShaderType type, string shaderName) => TryWith(type, shaderName, out _);

            public RendererShaderBuilder TryWith(ShaderType type, string shaderName, out bool succeeded)
            {
                succeeded = false;
                var shader = manager.getShader(type, shaderName);
                if (shader == null)
                    return this;

                succeeded = true;
                With(shader);

                return this;
            }

            public RendererShaderBuilder With(ReloadableShader shader)
            {
                shaders.Add(shader);
                return this;
            }
        }

        private ReloadableShader? getShader(ShaderType type, string shaderName)
        {
            shaders[type].TryGetValue(shaderName, out var shader);
            return shader;
        }
    }
}
