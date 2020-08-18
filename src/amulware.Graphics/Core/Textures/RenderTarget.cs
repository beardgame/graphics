using System;
using System.Linq;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.Textures
{
    public sealed class RenderTarget : IDisposable
    {
        private readonly int handle;

        public RenderTarget()
        {
            GL.GenFramebuffers(1, out handle);
        }

        public Target Bind(FramebufferTarget target = FramebufferTarget.DrawFramebuffer)
        {
            return new Target(this, target);
        }

        public readonly struct Target : IDisposable
        {
            private readonly FramebufferTarget target;

            internal Target(RenderTarget renderTarget, FramebufferTarget target)
            {
                this.target = target;
                GL.BindFramebuffer(target, renderTarget.handle);
            }

            public void SetColorAttachments(params Texture[] textures)
            {
                var attachments = new DrawBuffersEnum[textures.Length];
                foreach (var index in Enumerable.Range(0, textures.Length))
                {
                    Attach(FramebufferAttachment.ColorAttachment0 + index, textures[index]);
                    attachments[index] = DrawBuffersEnum.ColorAttachment0 + index;
                }
                GL.DrawBuffers(attachments.Length, attachments);
            }

            public void Attach(
                FramebufferAttachment attachment,
                Texture texture,
                TextureTarget textureTarget = TextureTarget.Texture2D)
            {
                GL.FramebufferTexture2D(target, attachment, textureTarget, texture.Handle, 0);
            }

            public void Dispose()
            {
                GL.BindFramebuffer(target, 0);
            }
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(handle);
        }
    }
}