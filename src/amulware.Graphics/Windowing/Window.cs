using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Windowing.GraphicsLibraryFramework;

namespace amulware.Graphics.Windowing
{
    public abstract class Window : NativeWindow
    {
        protected Window(NativeWindowSettings settings)
            : base(settings)
        {
            IsVisible = true;
        }

        public void Run()
        {
            OnLoad();

            OnResize(new ResizeEventArgs(Size));

            while (true)
            {
                ProcessEvents();

                OnUpdate();

                OnRender();
            }
        }

        protected abstract void OnLoad();

        protected abstract void OnUpdate();

        protected abstract void OnRender();

        protected void SwapBuffers()
        {
            unsafe
            {
                GLFW.SwapBuffers(WindowPtr);
            }
        }
    }
}
