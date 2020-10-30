using System;
using System.Collections.Immutable;
using amulware.Graphics.Pipelines.Context;
using amulware.Graphics.Pipelines.Steps;
using amulware.Graphics.PostProcessing;
using amulware.Graphics.Rendering;
using amulware.Graphics.RenderSettings;
using amulware.Graphics.ShaderManagement;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using static OpenToolkit.Graphics.OpenGL.ClearBufferMask;

namespace amulware.Graphics.Pipelines
{
    public static partial class Pipeline<TState>
    {
        public static IPipeline<TState> ClearColor(Color color = default)
        {
            return clear(ColorBufferBit, GL.ClearColor, (System.Drawing.Color) color);
        }

        public static IPipeline<TState> ClearColor(float red, float green, float blue, float alpha)
        {
            return clear(ColorBufferBit, GL.ClearColor, new Color4(red, green, blue, alpha));
        }

        public static IPipeline<TState> ClearDepth(double depth = 1)
        {
            return clear(DepthBufferBit, GL.ClearDepth, depth);
        }

        private static IPipeline<TState> clear<T>(ClearBufferMask mask, System.Action<T> set, T value)
            => new Clear<TState, T>(mask, set, value);

        public static IPipeline<TState> PostProcess(IRendererShader shaderProgram, out IDisposable disposable,
            params IRenderSetting[] renderSettings)
        {
            var postProcessor = PostProcessor.From(renderSettings);
            shaderProgram.UseOnRenderer(postProcessor);
            disposable = postProcessor;
            return new PostProcess<TState>(postProcessor);
        }

        public static IPipeline<TState> Render(IRenderer renderer)
        {
            return new Render<TState>(renderer);
        }

        public static IPipeline<TState> Render(Func<TState, IRenderer> getRenderer)
        {
            return new Render<TState>(getRenderer);
        }

        public static IPipeline<TState> WithContext(System.Action<PipelineContextBuilder<TState>> setup, IPipeline<TState> step)
        {
            var builder = new PipelineContextBuilder<TState>();
            setup(builder);
            return WithContext(builder, step);
        }

        public static IPipeline<TState> WithContext(PipelineContextBuilder<TState> configuredContextBuilder, IPipeline<TState> step)
        {
            return new WithContext<TState>(configuredContextBuilder.Build(), step);
        }

        public static IPipeline<TState> Resize(Func<TState, Vector2i> getSize, params PipelineTextureBase[] textures)
        {
            return new Resize<TState>(getSize, textures.ToImmutableArray());
        }
    }
}
