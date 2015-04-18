using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents an OpenGL framebuffer object that can be rendered to.
    /// </summary>
    sealed public class RenderTarget : IDisposable
    {
        #region Fields

        private readonly int handle;

        #endregion

        #region Properties

        /// <summary>
        /// The handle of the OpenGL framebuffer object associated with this render target
        /// </summary>
        public int Handle { get { return this.handle; } }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTarget"/> class.
        /// </summary>
        public RenderTarget()
        {
            GL.GenFramebuffers(1, out this.handle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTarget"/> class assigning a texture to its default color attachment.
        /// </summary>
        /// <param name="texture">The texture to attach.</param>
        public RenderTarget(Texture texture)
            : this()
        {
            this.Attach(FramebufferAttachment.ColorAttachment0, texture);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attaches a <see cref="Texture"/> to the specified <see cref="FramebufferAttachment"/>, so it can be rendered to.
        /// </summary>
        /// <param name="attachment">The attachment.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="target">Texture target of the attachment.</param>
        public void Attach(FramebufferAttachment attachment, Texture texture, TextureTarget target = TextureTarget.Texture2D)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, this.handle);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, attachment, target, texture, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Casts the <see cref="RenderTarget"/> to its OpenGL framebuffer object handle, for easy use with OpenGL functions.
        /// </summary>
        /// <remarks>Null is cast to 0.</remarks>
        static public implicit operator int(RenderTarget rendertarget)
        {
            if (rendertarget == null)
                return 0;
            return rendertarget.Handle;
        }

        #endregion

        #region Disposing

        private bool disposed;

        /// <summary>
        /// Disposes of the render target and deletes the underlying GL object.
        /// </summary>
        public void Dispose()
        {
            this.dispose();
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return; 

            int handle = this.Handle;
            GL.DeleteFramebuffers(1, ref handle);

            this.disposed = true;
        }

        ~RenderTarget()
        {
            this.dispose();
        }

        #endregion
    }
}
