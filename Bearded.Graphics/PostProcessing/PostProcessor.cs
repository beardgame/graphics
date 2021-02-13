using System.Collections.Generic;
using System.Linq;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Bearded.Graphics.PostProcessing
{
    public sealed class PostProcessor : IRenderer
    {
        private readonly Renderer internalRenderer;
        private readonly Buffer<PostProcessingVertexData> vertices;

        public static PostProcessor From(params IRenderSetting[] settings)
            => From(settings.AsEnumerable());

        public static PostProcessor From(IEnumerable<IRenderSetting> settings)
        {
            var (renderable, vertices) = makeRenderable();

            var renderer = Renderer.From(renderable, settings);

            return new PostProcessor(renderer, vertices);
        }

        public static PostProcessor From(ShaderProgram shaderProgram, params IRenderSetting[] settings)
            => From(shaderProgram, settings.AsEnumerable());

        public static PostProcessor From(ShaderProgram shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            var (renderable, vertices) = makeRenderable();

            var renderer = Renderer.From(renderable, shaderProgram, settings);

            return new PostProcessor(renderer, vertices);
        }

        private static (IRenderable, Buffer<PostProcessingVertexData>) makeRenderable()
        {
            var vertices = new Buffer<PostProcessingVertexData>();

            using (var target = vertices.Bind())
            {
                target.Upload(
                    new[]
                    {
                        new PostProcessingVertexData(new Vector2(-1, -1)),
                        new PostProcessingVertexData(new Vector2(1, -1)),
                        new PostProcessingVertexData(new Vector2(-1, 1)),
                        new PostProcessingVertexData(new Vector2(1, 1))
                    });
            }

            var renderable = Renderable.ForVertices(vertices, PrimitiveType.TriangleStrip);

            return (renderable, vertices);
        }

        private PostProcessor(Renderer internalRenderer, Buffer<PostProcessingVertexData> vertices)
        {
            this.internalRenderer = internalRenderer;
            this.vertices = vertices;
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            internalRenderer.SetShaderProgram(program);
        }

        public void Render()
        {
            internalRenderer.Render();
        }

        public void Dispose()
        {
            internalRenderer.Dispose();
            vertices.Dispose();
        }
    }
}
