using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace amulware.Graphics.ShaderManagement
{
    sealed public class ReloadableShaderProgram : ISurfaceShader
    {
        private readonly ReadOnlyCollection<ReloadableShader> shaders;
        private readonly ReplaceableShaderProgram program = new ReplaceableShaderProgram(null);

        public ReloadableShaderProgram(params ReloadableShader[] shaders)
            : this((IEnumerable<ReloadableShader>)shaders)
        {
        }

        public ReadOnlyCollection<ReloadableShader> Shaders
        {
            get { return this.shaders; }
        }

        public ReloadableShaderProgram(IEnumerable<ReloadableShader> shaders)
        {
            this.shaders = shaders.ToList().AsReadOnly();
            this.Reload();
        }
        
        public void Reload()
        {
            var program = new ShaderProgram(this.shaders.Select(s => s.Shader));
            this.program.SetProgram(program);
        }

        public bool ReloadIfContains(ReloadableShader shader)
        {
            var contains = this.shaders.Contains(shader);
            if (!contains)
                return false;
            this.Reload();
            return true;
        }

        public bool ReloadIfContainsAny(HashSet<ReloadableShader> shaders)
        {
            var contains = this.shaders.Any(shaders.Contains);
            if (!contains)
                return false;
            this.Reload();
            return true;
        }

        public void UseOnSurface(Surface surface)
        {
            this.program.UseOnSurface(surface);
        }

        public void RemoveFromSurface(Surface surface)
        {
            this.program.RemoveFromSurface(surface);
        }
    }
}
