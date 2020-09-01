using System;
using System.ComponentModel;
using System.Drawing;
using amulware.Graphics.Rendering;
using amulware.Graphics.RenderSettings;
using amulware.Graphics.Shading;
using amulware.Graphics.Text;
using amulware.Graphics.Textures;
using amulware.Graphics.Windowing;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;

namespace amulware.Graphics.Examples.Text
{
    sealed class GameWindow : Window
    {
        private ShaderProgram shaderProgram = null!;
        private TextDrawer<UVVertexData> textDrawer = null!;
        private BatchedRenderer renderer = null!;
        private Texture fontTexture = null!;

        private int width;
        private int height;
        private bool needsResize;

        public GameWindow()
            : base(
                new NativeWindowSettings
                    {
                        API = ContextAPI.OpenGL,
                        APIVersion = new Version(3, 2),
                        Title = "Text example",
                        WindowState = WindowState.Normal,
                        Size = new Vector2i(1280, 720)
                    }
                )
        {
        }

        protected override void OnLoad()
        {
            var systemFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 64, GraphicsUnit.Pixel);
            var (textureData, font) = FontFactory.From(systemFont, 1);
            fontTexture = new Texture();
            textureData.PopulateTexture(fontTexture);
            using(var target = fontTexture.Bind())
            {
                target.SetFilterMode(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Nearest);
                target.GenerateMipmap();
            }

            textDrawer = new TextDrawer<UVVertexData>(font, (p, uv) => new UVVertexData(p, uv));

            var renderable = textDrawer.ToRenderable();

            shaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("text.vs"), ShaderFactory.Fragment.FromFile("text.fs"));

            renderer = BatchedRenderer.From(renderable, shaderProgram,
                new TextureUniform("fontTexture", TextureUnit.Texture0, fontTexture));

            var textBrush = textDrawer.WithUnits(Vector3.UnitX * 2 / 1280, -Vector3.UnitY * 2 / 720);

            textBrush.DrawLine(Vector3.Zero, "Hello World!", 64, 0.5f, 0.5f);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            (width, height) = (e.Width, e.Height);
            needsResize = true;
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            prepareForFrame();

            // Renders the renderable to the current render target.
            renderer.Render();

            // Since we do double-buffered rendering, we swap the two buffers when we're done rendering our current
            // frame.
            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs obj)
        {
            // Disposing isn't really necessary, but if you stop using an object midway through your application run, it
            // prevents memory leaks.
            renderer.Dispose();
            textDrawer.Dispose();
            shaderProgram.Dispose();
            fontTexture.Dispose();

            base.OnClosing(obj);
        }

        private void prepareForFrame()
        {
            if (needsResize)
            {
                GL.Viewport(0, 0, width, height);
            }

            // Clear the entire render target, using black as clear color.
            var argb = Color.DarkSlateBlue;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
    }
}
