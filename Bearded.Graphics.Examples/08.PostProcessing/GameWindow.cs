using System;
using System.ComponentModel;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.PostProcessing;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;
using Bearded.Graphics.Shapes;
using Bearded.Graphics.Textures;
using Bearded.Graphics.Windowing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Bearded.Graphics.Examples.PostProcessing
{
    sealed class GameWindow : Window
    {
        private Texture texture = null!;
        private RenderTarget renderTarget = null!;

        private ShaderProgram shapeShaderProgram = null!;
        private Renderer shapeRenderer = null!;
        private readonly Matrix4Uniform viewMatrix = new Matrix4Uniform("view", Matrix4.Identity);
        private readonly Matrix4Uniform projectionMatrix = new Matrix4Uniform("projection", Matrix4.Identity);

        private ShaderProgram postProcessShader = null!;
        private PostProcessor postProcessor = null!;

        private IndexedTrianglesMeshBuilder<ColorVertexData> meshBuilder = null!;
        private readonly Vector2Uniform pixelSizeUniform = new Vector2Uniform("pixelSize", Vector2.Zero);

        private bool resizeNeeded;
        private int width;
        private int height;

        protected override NativeWindowSettings GetSettings()
        {
            return new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Title = "PostProcessing example",
                WindowState = WindowState.Normal,
                Size = new Vector2i(1280, 720)
            };
        }

        protected override void OnLoad()
        {
            texture = Texture.Empty(1, 1, PixelInternalFormat.Rgba);

            renderTarget = new RenderTarget();
            // ReSharper disable once ConvertToUsingDeclaration
            using (var target = renderTarget.Bind())
            {
                target.SetColorAttachments(texture);
            }

            shapeShaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("geometry.vs"), ShaderFactory.Fragment.FromFile("geometry.fs"));

            meshBuilder = new IndexedTrianglesMeshBuilder<ColorVertexData>();

            var shapeDrawer =
                new ShapeDrawer3<ColorVertexData, Color>(meshBuilder, (xyz, color) => new ColorVertexData(xyz, color));
            shapeDrawer.DrawCube(Vector3.Zero, 1f, Color.Aqua);

            var shapeRenderable = meshBuilder.ToRenderable();
            shapeRenderer = Renderer.From(shapeRenderable, shapeShaderProgram, viewMatrix, projectionMatrix);

            viewMatrix.Value = Matrix4.LookAt(new Vector3(0, 1, -2), Vector3.Zero, Vector3.UnitY);

            postProcessShader = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("postprocess.vs"), ShaderFactory.Fragment.FromFile("postprocess.fs"));

            postProcessor = PostProcessor.From(postProcessShader,
                new TextureUniform("inTexture", TextureUnit.Texture0, texture), pixelSizeUniform);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            resizeNeeded = true;
            (width, height) = (e.Width, e.Height);

            projectionMatrix.Value = Matrix4.CreatePerspectiveFieldOfView(
                MathF.PI / 4, (float) e.Width / e.Height, .1f, 100f);
            pixelSizeUniform.Value = new Vector2(1f / e.Width, 1f / e.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            var transform = Matrix4.CreateRotationY(e.ElapsedTimeInSf);

            viewMatrix.Value = transform * viewMatrix.Value;
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            if (resizeNeeded)
            {
                GL.Viewport(0, 0, width, height);

                using (var target = texture.Bind())
                {
                    target.Resize(width, height);
                }

                resizeNeeded = false;
            }

            using (renderTarget.Bind())
            {
                prepareForFrame();

                shapeRenderer.Render();
            }

            postProcessor.Render();

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs obj)
        {
            shapeRenderer.Dispose();
            shapeShaderProgram.Dispose();

            postProcessor.Dispose();
            postProcessShader.Dispose();

            meshBuilder.Dispose();

            texture.Dispose();
            renderTarget.Dispose();

            base.OnClosing(obj);
        }

        private static void prepareForFrame()
        {
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
