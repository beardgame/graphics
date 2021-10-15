using System;
using System.ComponentModel;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;
using Bearded.Graphics.Shapes;
using Bearded.Graphics.Windowing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Bearded.Graphics.Examples.RenderSettings
{
    sealed class GameWindow : Window
    {
        private ShaderProgram shapeShaderProgram = null!;
        private Renderer shapeRenderer = null!;
        private IndexedTrianglesMeshBuilder<ColorVertexData> meshBuilder = null!;

        // These are the various IRenderSettings that we use to control how a renderer behaves
        // In this case they are all uniforms which get send to the shader program,
        // matched by string name (see at the top of the shader code)
        private readonly Matrix4Uniform viewMatrix = new("view");
        private readonly Matrix4Uniform projectionMatrix = new("projection");
        private readonly Vector3Uniform lightPosition = new("lightPosition");
        // We can give these uniforms a value on construction, but the value is mutable in either case
        private readonly FloatUniform lightRadius = new("lightRadius", 1f);
        private readonly ColorUniform lightColor = new("lightColor", Color.IndianRed);
        private readonly ColorUniform ambientLightColor = new("ambientLightColor", Color.DarkSlateBlue);

        private bool resizeNeeded;
        private int width;
        private int height;

        protected override NativeWindowSettings GetSettings()
        {
            return new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Title = "RenderSettings example",
                WindowState = WindowState.Normal,
                Size = new Vector2i(1280, 720)
            };
        }

        protected override void OnLoad()
        {
            shapeShaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("geometry.vs"), ShaderFactory.Fragment.FromFile("geometry.fs"));

            meshBuilder = new IndexedTrianglesMeshBuilder<ColorVertexData>();

            var shapeDrawer =
                new ShapeDrawer3<ColorVertexData, Color>(meshBuilder, (xyz, color) => new ColorVertexData(xyz, color));
            shapeDrawer.DrawCube(Vector3.Zero, 1f, Color.White);

            var shapeRenderable = meshBuilder.ToRenderable();

            shapeRenderer = Renderer.From(
                shapeRenderable,
                shapeShaderProgram,
                // Here is where we specify what render settings to use
                viewMatrix,
                projectionMatrix,
                lightPosition,
                lightRadius,
                lightColor,
                ambientLightColor);

            viewMatrix.Value = Matrix4.LookAt(new Vector3(0, 1, -2), Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            resizeNeeded = true;
            (width, height) = (e.Width, e.Height);

            // We can change the value of the render settings, in this case only on resize
            projectionMatrix.Value = Matrix4.CreatePerspectiveFieldOfView(
                MathF.PI / 4, (float) e.Width / e.Height, .1f, 100f);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            // Here we change the view matrix and light position uniform values based on the time
            var transform = Matrix4.CreateRotationY(e.ElapsedTimeInSf * 0.1f);
            viewMatrix.Value = transform * viewMatrix.Value;

            var lightAngle = (float)e.TimeInS * 2;
            lightPosition.Value =
                Quaternion.FromAxisAngle(new Vector3(0, 1.5f, 1).Normalized(), lightAngle)
                    * Vector3.UnitX * 1.3f;
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            if (resizeNeeded)
            {
                GL.Viewport(0, 0, width, height);
                resizeNeeded = false;
            }

            prepareForFrame();

            // The renderer will use the last value set for each uniform,
            // but it will never change the value of the uniforms
            shapeRenderer.Render();

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs obj)
        {
            // We don't have to clean up regular render settings like uniforms,
            // they don't contain GL-state

            shapeRenderer.Dispose();
            shapeShaderProgram.Dispose();

            meshBuilder.Dispose();

            base.OnClosing(obj);
        }

        private static void prepareForFrame()
        {
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
