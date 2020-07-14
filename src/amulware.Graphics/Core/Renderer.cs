using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace amulware.Graphics
{
    // TODO: create replacement for batched vertex surface
    // TODO: create replacement for expanding vertex surface

    public class Renderer
    {
        private readonly IRenderable renderable;
        private readonly ImmutableArray<IRenderSetting> settings;

        private ShaderProgram shaderProgram;
        private VertexArray vertexArray;
        private ImmutableArray<IProgramRenderSetting> settingsForProgram;

        public static Builder NewBuilder(IRenderable renderable, ShaderProgram shaderProgram)
        {
            return new Builder(renderable, shaderProgram);
        }

        public sealed class Builder
        {
            private readonly IRenderable renderable;
            private readonly ShaderProgram shaderProgram;
            private readonly List<IRenderSetting> settings = new List<IRenderSetting>();

            public Builder(IRenderable renderable, ShaderProgram shaderProgram)
            {
                this.renderable = renderable;
                this.shaderProgram = shaderProgram;
            }

            public void Add(IRenderSetting setting)
            {
                settings.Add(setting);
            }

            public Renderer Build()
            {
                return new Renderer(renderable, shaderProgram, settings);
            }
        }

        public static Renderer From(IRenderable renderable, ShaderProgram shaderProgram)
        {
            return From(renderable, shaderProgram, Enumerable.Empty<IRenderSetting>());
        }

        public static Renderer From(IRenderable renderable, ShaderProgram shaderProgram, params IRenderSetting[] settings)
        {
            return From(renderable, shaderProgram, settings.AsEnumerable());
        }

        public static Renderer From(IRenderable renderable, ShaderProgram shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            return new Renderer(renderable, shaderProgram, settings);
        }

        private Renderer(IRenderable renderable, ShaderProgram shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            this.renderable = renderable;
            this.settings = settings.ToImmutableArray();

            setShaderProgram(shaderProgram);
        }

        // TODO: figure out where shader program replacing mutability comes in
        private void setShaderProgram(ShaderProgram program)
        {
            shaderProgram = program;
            vertexArray = VertexArray.For(renderable, program);
            settingsForProgram = settings.Select(s => s.ForProgram(program)).ToImmutableArray();
        }

        public void Render()
        {
            using (shaderProgram.Use())
            {
                foreach (var setting in settingsForProgram)
                {
                    setting.Set();
                }

                vertexArray.Render();

                // TODO: do we have to undo any settings? maybe we shouldn't have any like that
                // - texture samplers can behave funny though...
            }
        }
    }
}
