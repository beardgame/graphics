using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReloadableShaderProgram : IRendererShader
    {
        private readonly ReplaceableShaderProgram program = ReplaceableShaderProgram.CreateUninitialised();

        public ReadOnlyCollection<ReloadableShader> Shaders { get; }

        public static ReloadableShaderProgram LoadFrom(IEnumerable<ReloadableShader> shaders)
            => new ReloadableShaderProgram(shaders);

        public static ReloadableShaderProgram LoadFrom(params ReloadableShader[] shaders)
            => new ReloadableShaderProgram(shaders);

        private ReloadableShaderProgram(IEnumerable<ReloadableShader> shaders)
        {
            Shaders = shaders.ToList().AsReadOnly();
            Reload();
        }

        public bool ReloadIfContains(ReloadableShader shader)
        {
            return reloadIf(Shaders.Contains(shader));
        }

        public bool ReloadIfContainsAny(HashSet<ReloadableShader> shaders)
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
