using System;
using System.Collections.Generic;
using amulware.Graphics.Rendering;
using amulware.Graphics.RenderSettings;
using amulware.Graphics.Shading;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.PostProcessing
{
    public sealed class PostProcessor : IRenderer, IDisposable
    {
        private readonly Renderer internalRenderer;
        private readonly Buffer<PostProcessingVertexData> vertices;

        public static PostProcessor From(ShaderProgram shaderProgram, params IRenderSetting[] settings)
            => From(shaderProgram, (IEnumerable<IRenderSetting>) settings);

        public static PostProcessor From(ShaderProgram shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            var vertices = new Buffer<PostProcessingVertexData>();

            using (var target = vertices.Bind())
            {
                target.Upload(
                    new []
                    {
                        new PostProcessingVertexData(new Vector2(-1, -1)),
                        new PostProcessingVertexData(new Vector2(1, -1)),
                        new PostProcessingVertexData(new Vector2(-1, 1)),
                        new PostProcessingVertexData(new Vector2(1, 1))
                    });
            }

            var renderable = Renderable.ForVertices(vertices, PrimitiveType.TriangleStrip);

            var renderer = Renderer.From(renderable, shaderProgram, settings);

            return new PostProcessor(renderer, vertices);
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
