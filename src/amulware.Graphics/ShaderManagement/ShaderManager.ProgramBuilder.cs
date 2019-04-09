using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public sealed partial class ShaderManager
    {
        public ISurfaceShader MakeShaderProgram(string shaderName, string programName = null)
            => BuildShaderProgram()
                .TryAll(shaderName)
                .As(programName ?? shaderName);

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
                TryWith(ShaderType.VertexShader, shaderName);
                TryWith(ShaderType.GeometryShader, shaderName);
                TryWith(ShaderType.FragmentShader, shaderName);
                return this;
            }

            public ProgramBuilder WithVertexShader(string shaderName) => With(ShaderType.VertexShader, shaderName);

            public ProgramBuilder WithFragmentShader(string shaderName) => With(ShaderType.FragmentShader, shaderName);

            public ProgramBuilder WithGeometryShader(string shaderName) => With(ShaderType.GeometryShader, shaderName);

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

            public ISurfaceShader As(string programName)
            {
                var program = new ReloadableShaderProgram(shaders);
                manager.Add(program, programName);
                return program;
            }
        }
    }
}
