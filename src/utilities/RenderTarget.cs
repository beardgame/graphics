using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class RenderTarget : IDisposable
    {
        public readonly int Handle;

        public RenderTarget()
        {
            int handle;
            GL.GenFramebuffers(1, out handle);
            this.Handle = handle;
        }

        public RenderTarget(Texture texture)
            : this()
        {
            this.Attach(FramebufferAttachment.ColorAttachment0, texture);
        }

        public void Attach(FramebufferAttachment attachment, Texture texture)
        {

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, this.Handle);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, attachment, TextureTarget.Texture2D, texture, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

        }


        static public implicit operator int(RenderTarget rendertarget)
        {
            if (rendertarget == null)
                return 0;
            return rendertarget.Handle;
        }

        public void Dispose()
        {
            int handle = this.Handle;
            GL.DeleteFramebuffers(1, ref handle);
        }
    }
}
