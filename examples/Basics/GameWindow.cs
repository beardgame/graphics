using System;
using amulware.Graphics.Windowing;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;

namespace amulware.Graphics.Examples.Basics
{
    sealed class GameWindow : Window
    {
        // Matrix4Uniform (and similarly other _Uniform classes) represent an input for the shaders.
        // When a uniform is linked to a renderer, it will automatically assign the values of the uniform to the
        // respective uniform in the vertex shader (see geometry.vs for the corresponding inputs).
        private readonly Matrix4Uniform viewMatrix = new Matrix4Uniform("view", Matrix4.Identity);
        private readonly Matrix4Uniform projectionMatrix = new Matrix4Uniform("projection", Matrix4.Identity);

        private Renderer? renderer;

        public GameWindow()
            : base(
                new NativeWindowSettings
                    {
                        API = ContextAPI.OpenGL,
                        APIVersion = new Version(3, 2),
                        Title = "Basic example",
                        WindowState = WindowState.Normal,
                        Size = new Vector2i(1280, 720)
                    }
                )
        {
        }

        protected override void OnLoad()
        {
            // ...
            var buffer = new Buffer<ColorVertexData>();
            addTriangle(buffer);

            // ...
            var renderable = Renderable.ForVertices(buffer, PrimitiveType.Triangles);

            // The shader program contains the vertex and fragment shaders. It is assigned to a renderer.
            var shaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Geometry.FromFile("geometry.vs"), ShaderFactory.Fragment.FromFile("geometry.fs"));

            // ...
            renderer = Renderer.From(renderable, shaderProgram, viewMatrix, projectionMatrix);

            // Initialize a reasonable view matrix.
            viewMatrix.Value = Matrix4.LookAt(new Vector3(0, 0, -2), Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // OnResize is also called when the window is initially opened, so it is safe to initialize matrices here.

            // Use the simplest possible projection matrix: an orthographic projection.
            projectionMatrix!.Value = Matrix4.CreateOrthographic(e.Width, e.Height, .1f, 100f);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            prepareForFrame();

            // Renders the geometry to the current render target.
            renderer!.Render();

            // Since we do double-buffered rendering, we swap the two buffers when we're done rendering our current
            // frame.
            SwapBuffers();
        }

        private void prepareForFrame()
        {
            // Clear the entire render target, using black as clear color.
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        private void addTriangle(Buffer<ColorVertexData> buffer)
        {
            // You would often use a mesh builder or other helper class to draw shapes, such as rectangles, lines, and
            // even fonts or textures. However, for this example we add vertices to the buffer directly.
            using var target = buffer.Bind();

            // We draw a triangle by specifying the three corners and using the AddTriangle helper on the mesh builder
            // to take care of adding the right vertices and indices to the right buffers.
            target.Upload(new []
            {
                new ColorVertexData(-200, -100f, 0, Color.Red),
                new ColorVertexData(200, -100f, 0, Color.Green),
                new ColorVertexData(0, 200, 0, Color.Blue)
            });
        }
    }
}
