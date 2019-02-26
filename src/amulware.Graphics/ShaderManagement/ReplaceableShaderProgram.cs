using System.Collections.Generic;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReplaceableShaderProgram : ISurfaceShader
    {
        private ShaderProgram program;
        private readonly List<Surface> surfaces = new List<Surface>();

        public ReplaceableShaderProgram(ShaderProgram program)
        {
            this.program = program;
        }

        public void SetProgram(ShaderProgram newProgram)
        {
            program = newProgram;
            foreach (var surface in surfaces)
                surface.SetShaderProgram(newProgram);
        }

        public void UseOnSurface(Surface surface)
        {
            surfaces.Add(surface);
            surface.SetShaderProgram(program);
        }

        public void RemoveFromSurface(Surface surface)
        {
            surfaces.Remove(surface);
        }
    }
}
