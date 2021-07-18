using System;
using System.ComponentModel;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;
using Bearded.Graphics.Shapes;
using Bearded.Graphics.Windowing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Bearded.Graphics.Examples.Basics
{
    sealed class GameWindow : Window
    {
        // Matrix4Uniform (and similarly other _Uniform classes) represent static input for the shaders.
        // When a uniform is added to a renderer, it will automatically assign the values of the uniform to the
        // respective uniform in the shader (see geometry.vs for the corresponding inputs) when rendering.
        private readonly Matrix4Uniform viewMatrix = new Matrix4Uniform("view", Matrix4.Identity);
        private readonly Matrix4Uniform projectionMatrix = new Matrix4Uniform("projection", Matrix4.Identity);

        private Buffer<ColorVertexData> buffer = null!;
        private ShaderProgram shaderProgram = null!;
        private Renderer renderer = null!;

        protected override NativeWindowSettings GetSettings()
        {
            return new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Title = "Basic example",
                WindowState = WindowState.Normal,
                Size = new Vector2i(1280, 720)
            };
        }

        protected override void OnLoad()
        {
            // Create a buffer which will contain the vertices that we will render.
            buffer = new Buffer<ColorVertexData>();
            addTriangle(buffer);

            // Create a renderable wrapper for our buffer, interpreting it as a triangle list of vertices.
            var renderable = Renderable.ForVertices(buffer, PrimitiveType.Triangles);

            // The shader program contains the vertex and fragment shaders. It is assigned to a renderer.
            shaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("geometry.vs"), ShaderFactory.Fragment.FromFile("geometry.fs"));

            // Create a renderer to draw the renderable content with the given shader and settings like uniforms.
            renderer = Renderer.From(renderable, shaderProgram, viewMatrix, projectionMatrix);

            // Initialize a reasonable view matrix.
            viewMatrix.Value = Matrix4.LookAt(new Vector3(0, 0, -2), Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // OnResize is also called when the window is initially opened, so it is safe to initialize matrices here.

            // Use the simplest possible projection matrix: an orthographic projection.
            projectionMatrix.Value = Matrix4.CreateOrthographic(e.Width, e.Height, .1f, 100f);

            // We need to make sure we render to the full size of the window.
            GL.Viewport(0, 0, e.Width, e.Height);
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
            shaderProgram.Dispose();
            buffer.Dispose();

            base.OnClosing(obj);
        }

        private static void prepareForFrame()
        {
            // Clear the entire render target, using black as clear color.
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        private static void addTriangle(Buffer<ColorVertexData> buffer)
        {
            // You would often use a mesh builder or other helper class to draw shapes, such as rectangles, lines, and
            // even fonts or textured sprites. However, for this example we add vertices to the buffer directly.
            // The `using (var target = x.Bind())` pattern is a common one used to interface with low level OpenGL
            // functionality. Other, higher levels of abstractions hide these calls in an efficient manner.
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
