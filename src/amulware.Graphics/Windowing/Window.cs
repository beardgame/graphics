using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Windowing.GraphicsLibraryFramework;

namespace amulware.Graphics.Windowing
{
    public abstract class Window
    {
        private sealed class NativeWindowWrapper : NativeWindow
        {
            public NativeWindowWrapper(NativeWindowSettings settings)
                : base(settings)
            {
            }

            public void SwapBuffers()
            {
                unsafe { GLFW.SwapBuffers(WindowPtr); }
            }
        }

        private readonly NativeWindowWrapper window;

        protected Window(NativeWindowSettings settings)
        {
            window = new NativeWindowWrapper(settings)
            {
                IsVisible = true
            };
            window.Resize += OnResize;
        }

        public void Run()
        {
            OnLoad();

            OnResize(new ResizeEventArgs(window.Size));

            while (true)
            {
                window.ProcessEvents();

                if (!window.Exists || window.IsExiting)
                    return;

                OnUpdate();

                OnRender();
            }
        }

        protected abstract void OnLoad();

        protected abstract void OnResize(ResizeEventArgs eventArgs);

        protected abstract void OnUpdate();

        protected abstract void OnRender();

        protected void SwapBuffers()
        {
            window.SwapBuffers();
        }
    }
}
