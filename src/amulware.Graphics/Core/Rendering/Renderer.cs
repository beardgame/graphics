using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using amulware.Graphics.RenderSettings;
using amulware.Graphics.Shading;

namespace amulware.Graphics.Rendering
{
    public sealed class Renderer : IDisposable
    {
        private readonly IRenderable renderable;
        private readonly ImmutableArray<IRenderSetting> settings;

        private ShaderProgram shaderProgram = null!;
        private DrawCall drawCall = null!;
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

            SetShaderProgram(shaderProgram);
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            shaderProgram = program;
            drawCall?.Dispose();
            drawCall = renderable.MakeDrawCallFor(program);
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

                drawCall.Invoke();

                // TODO: do we have to undo any settings? maybe we shouldn't have any like that
                // - texture samplers can behave funny though...
            }
        }

        public void Dispose()
        {
            drawCall.Dispose();
        }
    }
}
