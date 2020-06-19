using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.Examples.Basics
{
    sealed class GameWindow : Program
    {
        private IndexedSurface<PrimitiveVertexData> surface;
        private Matrix4Uniform viewMatrix;
        private Matrix4Uniform projectionMatrix;

        public GameWindow()
            : base(
                1280, 720,
                GraphicsMode.Default,
                "Basic example",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, 2,
                GraphicsContextFlags.Default)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // A surface is a vertex buffer wrapper.
            // We use an indexed one, which is the most commonly used in amulware.Graphics.
            surface = new IndexedSurface<PrimitiveVertexData>();

            // Matrix4Uniform (and similarly other _Uniform classes) represent an input for the shaders.
            // When a uniform is linked to a surface, it will automatically assign the values of the uniform to the
            // respective uniform in the vertex shader (see geometry.vs for the corresponding inputs).
            viewMatrix = new Matrix4Uniform("view");
            projectionMatrix = new Matrix4Uniform("projection");
            surface.AddSettings(viewMatrix, projectionMatrix);

            // The shader program contains the vertex and fragment shaders used to render the vertices from the surface.
            surface.SetShaderProgram(ShaderProgram.FromFiles("geometry.vs", "geometry.fs"));

            // Initialize a reasonable view matrix.
            viewMatrix.Matrix = Matrix4.LookAt(new Vector3(0, 0, -2), Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnResize(EventArgs e)
        {
            // OnResize is also called when the window is initially opened, so it is safe to initialize matrices here.

            // Use the simplest possible projection matrix: an orthographic projection.
            projectionMatrix.Matrix = Matrix4.CreateOrthographic(Width, Height, .1f, 100f);

            base.OnResize(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            prepareForFrame();

            // The actual drawing code. This uploads all the necessary geometry to the graphics card.
            drawTriangle();

            // Renders the geometry to the buffer. By default, this will also clear the contents of the surface to
            // prepare it for next frame.
            surface.Render();

            // Since we do double-buffered rendering, we swap the two buffers when we're done rendering our current
            // frame.
            SwapBuffers();
        }

        private void prepareForFrame()
        {
            // Clear the entire buffer, using black as clear color.
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        private void drawTriangle()
        {
            // We draw a triangle by specifying the three corners and using the AddTriangle helper on the surface to
            // take care of adding the right vertices and indices to the right buffers.
            var v0 = new PrimitiveVertexData(-200, -100f, 0, Color.Red);
            var v1 = new PrimitiveVertexData(200, -100f, 0, Color.Green);
            var v2 = new PrimitiveVertexData(0, 200, 0, Color.Blue);

            // Usually you would use a geometry class to draw shapes, such as rectangles, lines, and even fonts or
            // textures. However, for this example we add vertices to the surface directly.
            surface.AddTriangle(v0, v1, v2);
        }
    }
}
