using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bearded.Graphics.Windowing
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
                unsafe
                {
                    GLFW.SwapBuffers(WindowPtr);
                }
            }

            public void DetachContextFromCallingThread()
            {
                unsafe
                {
                    GLFW.MakeContextCurrent(null);
                }
            }

            public void AttachContextToCallingThread()
            {
                unsafe
                {
                    GLFW.MakeContextCurrent(WindowPtr);
                }
            }
        }

        private NativeWindowWrapper window = null!;

        // TODO(#26): rewrite windowing natively and merge with input, instead of relying on OpenTK.Windowing.Desktop
        [Obsolete("Legacy implementation. There is no replacement yet.")]
        protected NativeWindow NativeWindow => window;

        private bool vsync = true;

        private bool forceResize = false;

        protected abstract NativeWindowSettings GetSettings();


        public void Run()
        {
            createWindowWithDetachedGLContext();
            startUpdateThread();
            runMainThreadLoop();
        }

        private void createWindowWithDetachedGLContext()
        {
            setupNativeWindow();
            OnLoad();
            TriggerResize();
            window.DetachContextFromCallingThread();
        }

        private void setupNativeWindow()
        {
            window = new NativeWindowWrapper(GetSettings())
            {
                IsVisible = true,
            };
            window.Resize += OnResize;
            window.Closing += OnClosing;
        }

        private void startUpdateThread()
        {
            var updateThread = new Thread(runGL)
            {
                Name = "GL Thread",
            };
            updateThread.Start();
        }

        private void runMainThreadLoop()
        {
            while (window.Exists && !window.IsExiting)
            {
                if (forceResize)
                {
                    OnResize(new ResizeEventArgs(window.Size));
                    forceResize = false;
                }
                
                window.ProcessEvents(TimeSpan.FromSeconds(1d / 200).TotalSeconds);
                OnUpdateUIThread();
            }
        }

        private void runGL()
        {
            window.AttachContextToCallingThread();

            var targetUpdatesPerSecond = 60;
            var targetDrawsPerSecond = 60;
            var maximumFrameTimeFactor = 3;

            var targetUpdateInterval = targetUpdatesPerSecond <= 0 ? 0 : 1 / targetUpdatesPerSecond;

            targetUpdateInterval = targetUpdatesPerSecond <= 0 ? 0 : 1 / targetUpdatesPerSecond;
            double targetRenderInterval = targetDrawsPerSecond <= 0 ? 0 : 1 / targetDrawsPerSecond;

            var maximumUpdateInterval = targetUpdateInterval == 0
                ? double.PositiveInfinity
                : targetUpdateInterval * maximumFrameTimeFactor;

            var updateEventArgs = new UpdateEventArgs(0);

            double lastTimerTime = 0;
            double gameSeconds = 0;
            double nextTargetRenderTime = 0;

            var gameTimer = Stopwatch.StartNew();

            while (true)
            {
                var thisTimerTime = gameTimer.Elapsed.TotalSeconds;
                var elapsedSeconds = thisTimerTime - lastTimerTime;
                lastTimerTime = thisTimerTime;

                var updateSeconds = Math.Min(elapsedSeconds, maximumUpdateInterval);

                gameSeconds += updateSeconds;

                if (!window.Exists || window.IsExiting)
                {
                    return;
                }

                updateEventArgs = new UpdateEventArgs(updateEventArgs, gameSeconds);

                OnUpdate(updateEventArgs);

                if (thisTimerTime >= nextTargetRenderTime)
                {
                    OnRender(updateEventArgs);
                    nextTargetRenderTime = thisTimerTime + targetRenderInterval;
                }

                if (vsync)
                {
                    continue;
                }

                var timeAfterFrame = gameTimer.Elapsed.TotalSeconds;

                var frameTime = timeAfterFrame - thisTimerTime;
                var waitTime = targetUpdateInterval - frameTime;
                if (waitTime > 0)
                    Thread.Sleep((int) (waitTime * 1000));
            }
        }

        protected void TriggerResize()
        {
            forceResize = true;
        }

        protected abstract void OnLoad();

        protected abstract void OnResize(ResizeEventArgs eventArgs);

        protected abstract void OnUpdate(UpdateEventArgs e);

        protected abstract void OnRender(UpdateEventArgs e);

        protected virtual void OnClosing(CancelEventArgs obj)
        {
        }

        protected virtual void OnUpdateUIThread()
        {
        }

        protected void SwapBuffers()
        {
            window.SwapBuffers();
        }

        protected void Close()
        {
            window.Close();
        }
    }
}
