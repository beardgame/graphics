using System;
using Bearded.Graphics.PostProcessing;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = Bearded.Graphics.Windowing.Window;

namespace Bearded.Graphics.Examples.Mandelbrot
{
    // Move around with arrow keys and zoom with Z/X
    sealed class GameWindow : Window
    {
        private (int Width, int Height) windowSize;
        private float scaleExponent = -1.3f;
        private readonly Vector2Uniform scale = new Vector2Uniform("scale");
        private readonly Vector2Uniform offset = new Vector2Uniform("offset");
        private PostProcessor renderer;

        protected override NativeWindowSettings GetSettings()
        {
            return new NativeWindowSettings
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Title = "Mandelbrot example",
                WindowState = WindowState.Normal,
                Size = new Vector2i(1280, 720)
            };
        }

        protected override void OnLoad()
        {
            var shaderProgram = ShaderProgram.FromShaders(
                ShaderFactory.Vertex.FromFile("postprocess.vs"),
                ShaderFactory.Fragment.FromFile("mandelbrot.fs")
                );
            renderer = PostProcessor.From(shaderProgram, offset, scale);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            windowSize = (e.Width, e.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            var keyboard = NativeWindow.KeyboardState;

            var zoom = (keyboard.IsKeyDown(Keys.Z) ? 1 : 0) + (keyboard.IsKeyDown(Keys.X) ? -1 : 0);

            var move = new Vector2(
                (keyboard.IsKeyDown(Keys.Right) ? 1 : 0) + (keyboard.IsKeyDown(Keys.Left) ? -1 : 0),
                (keyboard.IsKeyDown(Keys.Up) ? 1 : 0) + (keyboard.IsKeyDown(Keys.Down) ? -1 : 0)
            );

            scaleExponent += zoom * e.ElapsedTimeInSf;
            var distance = 1 / MathF.Pow(2, scaleExponent);

            scale.Value = new Vector2(distance, distance * windowSize.Height / windowSize.Width);
            offset.Value += move * distance * e.ElapsedTimeInSf;
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            GL.Viewport(0, 0, windowSize.Width, windowSize.Height);

            renderer.Render();

            SwapBuffers();
        }

    }
}
