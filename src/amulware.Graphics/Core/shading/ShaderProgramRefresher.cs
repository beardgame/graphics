using System.Collections.Generic;

namespace amulware.Graphics
{
    public sealed class ShaderProgramRefresher : ISurfaceShader
    {
        private ShaderProgram program;
        private readonly List<Surface> surfaces = new List<Surface>();

        public ShaderProgramRefresher(ShaderProgram program)
        {
            this.program = program;
        }

        public void UseOnSurface(Surface surface)
        {
            this.surfaces.Add(surface);
            surface.SetShaderProgram(this.program);
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            this.program = program;
            foreach (var surface in this.surfaces)
                surface.SetShaderProgram(program);
        }

        public void RemoveFromSurface(Surface surface)
        {
            this.surfaces.Remove(surface);
        }
    }
}
