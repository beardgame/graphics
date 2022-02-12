using System;
using System.Linq;
using Bearded.Graphics.Debugging;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Textures;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Pipelines
{
    public static partial class Pipeline
    {
        public static PipelineRenderTarget RenderTargetWithColors(string label, params PipelineTexture[] textures)
        {
            return withLabel(label, RenderTargetWithColors(textures));
        }

        public static PipelineRenderTarget RenderTargetWithDepthAndColors(
            string label, PipelineDepthTexture depth, params PipelineTexture[] textures)
        {
            return withLabel(label, RenderTargetWithDepthAndColors(depth, textures));
        }

        private static PipelineRenderTarget withLabel(string label, PipelineRenderTarget target)
        {
            KHRDebugExtension.Instance.SetObjectLabel(ObjectLabelIdentifier.Framebuffer, target.Handle, label);
            return target;
        }

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
            Action<Texture.Target>? setup = null,
            string? label = null)
        {
            var texture = Textures.Texture.Empty(width, height, pixelFormat);

            optionalTextureSetup(texture, setup, label);

            return new PipelineTexture(texture);
        }
        public static PipelineDepthTexture DepthTexture(
            PixelInternalFormat pixelFormat,
            int width = 1, int height = 1,
            Action<Texture.Target>? setup = null,
            string? label = null)
        {
            var texture = Textures.Texture.Empty(width, height, pixelFormat, PixelFormat.DepthComponent, PixelType.Float);

            optionalTextureSetup(texture, target =>
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.DepthTextureMode, (int) All.Intensity);
                setup?.Invoke(target);
            }, label);

            return new PipelineDepthTexture(texture);
        }

        private static void optionalTextureSetup(
            Texture texture, Action<Texture.Target>? setup, string? label)
        {
            if (setup != null)
            {
                using var target = texture.Bind();
                setup(target);
            }

            if (label != null)
            {
                KHRDebugExtension.Instance.SetObjectLabel(ObjectLabelIdentifier.Texture, texture.Handle, label);
            }
        }

        public static IRenderSetting TextureUniforms(
            params (PipelineTextureBase Texture, string UniformName)[] textures)
        {
            return new CompositeRenderSetting(
                textures.Select(
                    (t, i) => new TextureUniform(t.UniformName, TextureUnit.Texture0 + i, t.Texture.Texture))
            );
        }
    }
}
