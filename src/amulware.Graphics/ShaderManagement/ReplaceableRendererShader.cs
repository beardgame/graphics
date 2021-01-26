using System;
using System.Collections.Generic;
using amulware.Graphics.Rendering;
using amulware.Graphics.Shading;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReplaceableRendererShader : IRendererShader, IDisposable
    {
        private ShaderProgram? program;
        private readonly List<IRenderer> renderers = new List<IRenderer>();

        public static ReplaceableRendererShader CreateUninitialized() => new ReplaceableRendererShader(null!);

        public ReplaceableRendererShader(ShaderProgram? program)
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

        public void UseOnRenderer(IRenderer renderer)
        {
            renderers.Add(renderer);
            if (program != null)
                renderer.SetShaderProgram(program);
        }

        public void RemoveFromRenderer(IRenderer renderer)
        {
            renderers.Remove(renderer);
        }

        public void Dispose()
        {
            program?.Dispose();
        }
    }
}
