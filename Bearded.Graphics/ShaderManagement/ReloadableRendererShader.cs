using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.Shading;

namespace Bearded.Graphics.ShaderManagement
{
    public sealed class ReloadableRendererShader : IRendererShader, IDisposable
    {
        private readonly ReplaceableRendererShader program = ReplaceableRendererShader.CreateUninitialized();

        public ImmutableArray<IShaderProvider> Shaders { get; }

        public static ReloadableRendererShader LoadFrom(IEnumerable<IShaderProvider> shaders)
            => new ReloadableRendererShader(shaders);

        public static ReloadableRendererShader LoadFrom(params IShaderProvider[] shaders)
            => new ReloadableRendererShader(shaders);

        private ReloadableRendererShader(IEnumerable<IShaderProvider> shaders)
        {
            Shaders = shaders.ToImmutableArray();
            Reload();
        }

        public bool ReloadIfContains(IShaderProvider shader)
        {
            return reloadIf(Shaders.Contains(shader));
        }

        public bool ReloadIfContainsAny(ICollection<IShaderProvider> shaders)
        {
            return reloadIf(Shaders.Any(shaders.Contains));
        }

        private bool reloadIf(bool reload)
        {
            if (reload)
                Reload();

            return reload;
        }

        public void Reload()
        {
            var newProgram = ShaderProgram.FromShaders(Shaders.Select(s => s.Shader));
            program.SetProgram(newProgram, true);
        }

        public void UseOnRenderer(IRenderer renderer)
        {
            program.UseOnRenderer(renderer);
        }

        public void RemoveFromRenderer(IRenderer renderer)
        {
            program.RemoveFromRenderer(renderer);
        }

        public void Dispose()
        {
            program.Dispose();
        }
    }
}
