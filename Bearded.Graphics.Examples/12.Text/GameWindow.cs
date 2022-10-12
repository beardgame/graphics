using System;
using System.ComponentModel;
using System.Drawing;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;
using Bearded.Graphics.System.Drawing;
using Bearded.Graphics.Text;
using Bearded.Graphics.Textures;
using Bearded.Graphics.Windowing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Void = Bearded.Utilities.Void;
using SystemFont = System.Drawing.Font;

namespace Bearded.Graphics.Examples.Text
{
    sealed class GameWindow : Window
    {
        private ShaderProgram shaderProgram = null!;
        private ExpandingIndexedTrianglesMeshBuilder<UVVertexData> meshBuilder = null!;
        private BatchedRenderer renderer = null!;
        private Texture fontTexture = null!;

        private int width;
        private int height;
        private bool needsResize;

        protected override NativeWindowSettings GetSettings()
        {
            return new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Title = "Text example",
                WindowState = WindowState.Normal,
                Size = new Vector2i(1280, 720)
            };
        }

        protected override void OnLoad()
        {
            // The font factory allows us to extract the font information and construct a font texture using the
            // System.Drawing library.
            var systemFont = new SystemFont(FontFamily.GenericSansSerif, 64, GraphicsUnit.Pixel);
            var (textureData, font) = FontFactory.From(systemFont, 1);
            fontTexture = Texture.From(textureData, t =>
            {
                t.SetFilterMode(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Nearest);
                t.GenerateMipmap();
            });

            // We set up a mesh builder, and renderer as usual. We bind the font texture from the font loading into the
            // shader.
            meshBuilder = new ExpandingIndexedTrianglesMeshBuilder<UVVertexData>();

            var renderable = meshBuilder.ToRenderable();

            shaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("text.vs"), ShaderFactory.Fragment.FromFile("text.fs"));

            renderer = BatchedRenderer.From(renderable, shaderProgram,
                new TextureUniform("fontTexture", TextureUnit.Texture0, fontTexture));

            // The text drawer is a helper class to help us draw text assuming that it will be rendered with the right
            // font texture bound.
            var textDrawer =
                new TextDrawer<UVVertexData, Void>(font, meshBuilder, (p, uv, _) => new UVVertexData(p, uv));

            textDrawer.DrawLine(
                Vector3.Zero, "Hello World!",
                64, 0.5f, 0.5f,
                Vector3.UnitX * 2 / 1280, -Vector3.UnitY * 2 / 720,
                default);
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
            meshBuilder.Dispose();
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
