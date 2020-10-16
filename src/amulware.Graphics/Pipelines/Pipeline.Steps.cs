using System;
using System.Collections.Immutable;
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
    public static partial class Pipeline
    {
        public static IPipeline ClearColor(Color color = default)
        {
            return clear(ColorBufferBit, GL.ClearColor, (System.Drawing.Color) color);
        }

        public static IPipeline ClearDepth(double depth = 1)
        {
            return clear(DepthBufferBit, GL.ClearDepth, depth);
        }

        private static Clear<T> clear<T>(ClearBufferMask mask, Action<T> set, T value)
            => new Clear<T>(mask, set, value);

        public static IPipeline WithContext(Action<PipelineContextBuilder> setup, IPipeline step)
        {
            var builder = new PipelineContextBuilder();
            setup(builder);
            return new WithContext(builder.Build(), step);
        }

        public static IPipeline Resize(Func<Vector2i> getSize, params PipelineTextureBase[] textures)
        {
            return new Resize(getSize, textures.ToImmutableArray());
        }

        public static IPipeline PostProcess(IRendererShader shaderProgram, out IDisposable disposable,
            params IRenderSetting[] renderSettings)
        {
            var postProcessor = PostProcessor.From(renderSettings);
            shaderProgram.UseOnRenderer(postProcessor);
            disposable = postProcessor;
            return new PostProcess(postProcessor);
        }

        public static IPipeline Render(IRenderer renderer)
        {
            return new Render(renderer);
        }
    }
}
