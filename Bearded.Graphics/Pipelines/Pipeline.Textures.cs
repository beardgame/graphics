using System;
using System.Linq;
using Bearded.Graphics.Textures;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Pipelines
{
    public static partial class Pipeline
    {
        public static PipelineRenderTarget RenderTargetWithColors(params PipelineTexture[] textures)
        {
            return renderTarget(null, textures);
        }

        public static PipelineRenderTarget RenderTargetWithDepthAndColors(
            PipelineDepthTexture depth, params PipelineTexture[] textures)
        {
            return renderTarget(
                target => target.Attach(FramebufferAttachment.DepthAttachment, depth.Texture), textures);
        }

        private static PipelineRenderTarget renderTarget(
            Action<RenderTarget.Target>? additionalSetup, params PipelineTexture[] textures)
        {
            var renderTarget = new RenderTarget();

            using (var target = renderTarget.Bind())
            {
                target.SetColorAttachments(textures.Select(t => t.Texture).ToArray());
                additionalSetup?.Invoke(target);

                var status = target.CheckStatus();
                if (status != FramebufferErrorCode.FramebufferComplete)
                {
                    throw new InvalidOperationException($"Framebuffer incomplete: {status}");
                }
            }

            return new PipelineRenderTarget(renderTarget);
        }

        public static PipelineTexture Texture(
            PixelInternalFormat pixelFormat,
            int width = 1, int height = 1,
            Action<Texture.Target>? setup = null)
        {
            var texture = Textures.Texture.Empty(width, height, pixelFormat);

            if (setup != null)
            {
                using var target = texture.Bind();
                setup(target);
            }

            return new PipelineTexture(texture);
        }

        public static PipelineDepthTexture DepthTexture(
            PixelInternalFormat pixelFormat,
            int width = 1, int height = 1,
            Action<Texture.Target>? setup = null)
        {
            var texture = Textures.Texture.Empty(width, height, pixelFormat, PixelFormat.DepthComponent, PixelType.Float);

            using (var target = texture.Bind())
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.DepthTextureMode, (int) All.Intensity);
                setup?.Invoke(target);
            }

            return new PipelineDepthTexture(texture);
        }
    }
}
