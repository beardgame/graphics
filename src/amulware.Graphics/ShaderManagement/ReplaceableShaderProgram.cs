using System.Collections.Generic;

namespace amulware.Graphics.ShaderManagement
{
    sealed class ReplaceableShaderProgram : ISurfaceShader
    {
        private ShaderProgram program;
        private readonly List<Surface> surfaces = new List<Surface>();

        public ReplaceableShaderProgram(ShaderProgram program)
        {
            this.program = program;
        }

        public void SetProgram(ShaderProgram program)
        {
            this.program = program;
            foreach (var surface in this.surfaces)
                surface.SetShaderProgram(program);
        }

        public void UseOnSurface(Surface surface)
        {
            this.surfaces.Add(surface);
            surface.SetShaderProgram(this.program);
        }

        public void RemoveFromSurface(Surface surface)
        {
            this.surfaces.Remove(surface);
        }
    }
}
