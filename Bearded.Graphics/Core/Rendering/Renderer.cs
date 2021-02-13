using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;

namespace Bearded.Graphics.Rendering
{
    public sealed class Renderer : IRenderer
    {
        private readonly IRenderable renderable;
        private readonly ImmutableArray<IRenderSetting> settings;

        private (ShaderProgram Program, DrawCall DrawCall)? shader;
        private ImmutableArray<IProgramRenderSetting> settingsForProgram;

        public static Renderer From(IRenderable renderable)
        {
            return From(renderable, Enumerable.Empty<IRenderSetting>());
        }

        public static Renderer From(IRenderable renderable, params IRenderSetting[] settings)
        {
            return From(renderable, settings.AsEnumerable());
        }

        public static Renderer From(IRenderable renderable, IEnumerable<IRenderSetting> settings)
        {
            return new Renderer(renderable, null, settings);
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

        private Renderer(IRenderable renderable, ShaderProgram? shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            this.renderable = renderable;
            this.settings = settings.ToImmutableArray();

            if(shaderProgram != null)
                SetShaderProgram(shaderProgram);
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            shader?.DrawCall.Dispose();
            var drawCall = renderable.MakeDrawCallFor(program);
            shader = (program, drawCall);
            settingsForProgram = settings.Select(s => s.ForProgram(program)).ToImmutableArray();
        }

        public void Render()
        {
            if (shader == null)
                throw new InvalidOperationException("Must set renderer shader program before rendering.");

            var (program, drawCall) = shader.Value;

            using (program.Use())
            {
                foreach (var setting in settingsForProgram)
                {
                    setting.Set();
                }

                drawCall.Invoke();
            }
        }

        public void Dispose()
        {
            shader?.DrawCall.Dispose();
        }
    }
}
