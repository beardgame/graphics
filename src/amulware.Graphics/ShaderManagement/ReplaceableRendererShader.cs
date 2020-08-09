using System;
using System.Collections.Generic;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReplaceableRendererShader : IRendererShader, IDisposable
    {
        private ShaderProgram program;
        private readonly List<Renderer> renderers = new List<Renderer>();

        public static ReplaceableRendererShader CreateUninitialised() => new ReplaceableRendererShader(null!);

        public ReplaceableRendererShader(ShaderProgram program)
        {
            this.program = program;
        }

        public void SetProgram(ShaderProgram newProgram, bool disposePrevious = false)
        {
            if (disposePrevious)
                program?.Dispose();
            program = newProgram;
            foreach (var renderer in renderers)
                renderer.SetShaderProgram(newProgram);
        }

        public void UseOnRenderer(Renderer renderer)
        {
            renderers.Add(renderer);
            if (program != null)
                renderer.SetShaderProgram(program);
        }

        public void RemoveFromRenderer(Renderer renderer)
        {
            renderers.Remove(renderer);
        }

        public void Dispose()
        {
            program.Dispose();
        }
    }
}
