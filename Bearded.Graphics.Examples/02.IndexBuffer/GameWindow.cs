using System;
using System.ComponentModel;
using Bearded.Graphics.Rendering;
using Bearded.Graphics.Shading;
using Bearded.Graphics.Shapes;
using Bearded.Graphics.Windowing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Bearded.Graphics.Examples.IndexBuffer
{
    sealed class GameWindow : Window
    {
        private Buffer<ColorVertexData> vertexBuffer = null!;
        // We can use unsigned shorts or unsigned integers, depending on how many indices we need.
        // If we don't need more than 16^2, using ushort is typically better since it only uses half the memory.
        private Buffer<ushort> indexBuffer = null!;
        private ShaderProgram shaderProgram = null!;
        private Renderer renderer = null!;

        // Simple state for our animation below.
        private Vector2 quadPosition = Vector2.Zero;
        private Vector2 quadVelocity = new Vector2(0.8f, 0.7f);

        protected override NativeWindowSettings GetSettings()
        {
            return new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Title = "IndexBuffer example",
                WindowState = WindowState.Normal,
                Size = new Vector2i(1280, 720)
            };
        }

        protected override void OnLoad()
        {
            vertexBuffer = new Buffer<ColorVertexData>();
            indexBuffer = new Buffer<ushort>();

            // Since we want to render exactly one quad, we can upload indices here once and use them for each render
            // call below.
            uploadQuadIndices(indexBuffer);

            // When creating our renderable, the PrimitiveType defines how the indices are interpreted and translated
            // into geometry.
            var renderable = Renderable.ForVerticesAndIndices(vertexBuffer, indexBuffer, PrimitiveType.Triangles);

            shaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("geometry.vs"), ShaderFactory.Fragment.FromFile("geometry.fs"));

            renderer = Renderer.From(renderable, shaderProgram);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            // You would typically call your game update code here. In this case we simply update our animation.
            quadPosition += quadVelocity * e.ElapsedTimeInSf;

            // We use the fact that OpenGL uses a [-1, 1] view frustrum to make these collision checks easy here.
            if (Math.Abs(quadPosition.X) > 0.9)
                quadVelocity.X *= -1;

            if (Math.Abs(quadPosition.Y) > 0.9)
                quadVelocity.Y *= -1;
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            prepareForFrame();

            // We already uploaded our indices above. To draw our quad at the updated position we upload vertices
            // with updated positions here every frame.
            uploadVertices(vertexBuffer);

            renderer.Render();

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs obj)
        {
            renderer.Dispose();
            shaderProgram.Dispose();
            indexBuffer.Dispose();
            vertexBuffer.Dispose();

            base.OnClosing(obj);
        }

        private static void prepareForFrame()
        {
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        // How you want to lay out your vertices and triangles depends highly on what and how you want to render.
        // In the functions below, the quad is laid out as follows.
        //
        // Y
        // ^
        // | p3--p2
        // | |  / |
        // | | /  |
        // | p0--p1
        // +---------> X

        private void uploadVertices(Buffer<ColorVertexData> buffer)
        {
            const float radius = 0.1f;

            var p0 = new Vector3(quadPosition.X - radius, quadPosition.Y - radius, 0);
            var p1 = new Vector3(quadPosition.X + radius, quadPosition.Y - radius, 0);
            var p2 = new Vector3(quadPosition.X + radius, quadPosition.Y + radius, 0);
            var p3 = new Vector3(quadPosition.X - radius, quadPosition.Y + radius, 0);

            using var target = buffer.Bind();

            // We create a new array here on every call. This can create a log of garbage that you probably want to
            // avoid in a real app/game by re-using the array or by using an abstraction that does this for you,
            // like MeshBuilder.
            target.Upload(new []
            {
                new ColorVertexData(p0, Color.Red),
                new ColorVertexData(p1, Color.Lime),
                new ColorVertexData(p2, Color.Blue),
                new ColorVertexData(p3, Color.Purple),
            });
        }

        private void uploadQuadIndices(Buffer<ushort> buffer)
        {
            using var target = buffer.Bind();

            // Here we upload the 0-based indices for our two triangles that form a quad. Since we created
            // the renderable with PrimitiveType.Triangle, we need three indices for each triangle.
            target.Upload(new ushort[]
            {
                0, 1, 2,
                0, 2, 3
            });
        }
    }
}
