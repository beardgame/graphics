using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenToolkit.Graphics.OpenGL;
using static OpenToolkit.Graphics.OpenGL.ShaderType;

namespace amulware.Graphics.ShaderManagement
{
    public sealed partial class ShaderManager
    {
        public IRendererShader MakeShaderProgram(string shaderName, string programName = null)
            => BuildShaderProgram()
                .TryAll(shaderName)
                .BuildAs(programName ?? shaderName);

        public ProgramBuilder BuildShaderProgram()
            => new ProgramBuilder(this);

        public sealed class ProgramBuilder
        {
            private readonly ShaderManager manager;
            private readonly List<ReloadableShader> shaders = new List<ReloadableShader>();

            public ProgramBuilder(ShaderManager manager)
            {
                this.manager = manager;
            }

            public ProgramBuilder TryAll(string shaderName)
            {
                TryWith(ComputeShader, shaderName);
                TryWith(FragmentShader, shaderName);
                TryWith(GeometryShader, shaderName);
                TryWith(TessControlShader, shaderName);
                TryWith(TessEvaluationShader, shaderName);
                TryWith(VertexShader, shaderName);
                return this;
            }

            public ProgramBuilder WithVertexShader(string shaderName) => With(VertexShader, shaderName);

            public ProgramBuilder WithFragmentShader(string shaderName) => With(FragmentShader, shaderName);

            public ProgramBuilder WithGeometryShader(string shaderName) => With(GeometryShader, shaderName);

            public ProgramBuilder With(ShaderType type, string shaderName)
            {
                TryWith(type, shaderName, out var succeeded);
                if (!succeeded)
                    throw new ArgumentException($"{type} with name '{shaderName}' not found.");
                return this;
            }

            public ProgramBuilder TryWith(ShaderType type, string shaderName) => TryWith(type, shaderName, out _);

            public ProgramBuilder TryWith(ShaderType type, string shaderName, out bool succeeded)
            {
                succeeded = false;
                var shader = manager.getShader(type, shaderName);
                if (shader == null)
                    return this;

                succeeded = true;
                With(shader);

                return this;
            }

            public ProgramBuilder With(ReloadableShader shader)
            {
                shaders.Add(shader);
                return this;
            }

            public IRendererShader BuildAs(string programName)
            {
                manager.throwIfShaderProgramNameAlreadyTaken(programName);
                var program = ReloadableRendererShader.LoadFrom(shaders);
                manager.addProgram(program, programName);
                return program;
            }
        }

        private void addProgram(ReloadableRendererShader rendererShader, string name)
        {
            throwIfAnyShadersAreNotKnown(rendererShader, name);

            programs.Add(name, rendererShader);

            registerProgramForItsShaders(rendererShader);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        [Conditional("DEBUG")]
        private void throwIfAnyShadersAreNotKnown(ReloadableRendererShader rendererShader, string name)
        {
            if (rendererShader.Shaders.FirstOrDefault(s => !shaderNames.ContainsKey(s)) is { } shader)
                throw new InvalidOperationException(
                    $"Tried adding shader program with unknown {shader.Shader.Type} under name '{name}." +
                    " All shaders must be added first.");
        }

        private void throwIfShaderProgramNameAlreadyTaken(string name)
        {
            if (programs.ContainsKey(name))
                throw new ArgumentException($"Tried adding shader program with name '{name} which is already taken.");

        }

        private ReloadableShader? getShader(ShaderType type, string shaderName)
        {
            shaders[type].TryGetValue(shaderName, out var shader);
            return shader;
        }
    }
}
