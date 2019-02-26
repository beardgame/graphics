using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReloadableShaderProgram : ISurfaceShader
    {
        private readonly ReplaceableShaderProgram program = new ReplaceableShaderProgram(null);

        public ReloadableShaderProgram(params ReloadableShader[] shaders)
            : this((IEnumerable<ReloadableShader>)shaders)
        {
        }

        public ReadOnlyCollection<ReloadableShader> Shaders { get; }

        public ReloadableShaderProgram(IEnumerable<ReloadableShader> shaders)
        {
            Shaders = shaders.ToList().AsReadOnly();
            Reload();
        }
        
        public void Reload()
        {
            var newProgram = new ShaderProgram(Shaders.Select(s => s.Shader));
            program.SetProgram(newProgram);
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

        public void UseOnSurface(Surface surface)
        {
            program.UseOnSurface(surface);
        }

        public void RemoveFromSurface(Surface surface)
        {
            program.RemoveFromSurface(surface);
        }
    }
}
