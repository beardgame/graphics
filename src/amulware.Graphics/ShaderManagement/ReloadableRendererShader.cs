using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReloadableRendererShader : IRendererShader
    {
        private readonly ReplaceableRendererShader program = ReplaceableRendererShader.CreateUninitialised();

        public ReadOnlyCollection<IShaderProvider> Shaders { get; }

        public static ReloadableRendererShader LoadFrom(IEnumerable<IShaderProvider> shaders)
            => new ReloadableRendererShader(shaders);

        public static ReloadableRendererShader LoadFrom(params IShaderProvider[] shaders)
            => new ReloadableRendererShader(shaders);

        private ReloadableRendererShader(IEnumerable<IShaderProvider> shaders)
        {
            Shaders = shaders.ToList().AsReadOnly();
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
            program.SetProgram(newProgram);
        }

        public void UseOnRenderer(Renderer renderer)
        {
            program.UseOnRenderer(renderer);
        }

        public void RemoveFromRenderer(Renderer renderer)
        {
            program.RemoveFromRenderer(renderer);
        }
    }
}
