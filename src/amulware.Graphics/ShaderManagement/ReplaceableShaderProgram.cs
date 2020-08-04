using System.Collections.Generic;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReplaceableShaderProgram : IRendererShader
    {
        private ShaderProgram program;
        private readonly List<Renderer> renderers = new List<Renderer>();

        public static ReplaceableShaderProgram CreateUninitialised() => new ReplaceableShaderProgram(null!);

        public ReplaceableShaderProgram(ShaderProgram program)
        {
            this.program = program;
        }

        public void SetProgram(ShaderProgram newProgram)
        {
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
    }
}
