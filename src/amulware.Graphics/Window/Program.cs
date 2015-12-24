using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Bearded.Utilities.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using Encoder = System.Drawing.Imaging.Encoder;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace amulware.Graphics
{
    public class Program : NativeWindow, IGameWindow
    {
        #region --- Fields ---

        object exit_lock = new object();

        IGraphicsContext glContext;

        bool isExiting = false;

        private double targetUpdateInterval;
        private bool vsync;

        #endregion

        #region --- Contructors ---


        public Program()
            : this(640, 480, GraphicsMode.Default, "OpenTK Game Window", 0, DisplayDevice.Default) { }

        public Program(int width, int height)
            : this(width, height, GraphicsMode.Default, "OpenTK Game Window", 0, DisplayDevice.Default) { }

        public Program(int width, int height, GraphicsMode mode)
            : this(width, height, mode, "OpenTK Game Window", 0, DisplayDevice.Default) { }

        public Program(int width, int height, GraphicsMode mode, string title)
            : this(width, height, mode, title, 0, DisplayDevice.Default) { }

        public Program(int width, int height, GraphicsMode mode, string title, GameWindowFlags options)
            : this(width, height, mode, title, options, DisplayDevice.Default) { }

        public Program(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device)
            : this(width, height, mode, title, options, device, 1, 0, GraphicsContextFlags.Default)
        { }

        public Program(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
            int major, int minor, GraphicsContextFlags flags)
            : this(width, height, mode, title, options, device, major, minor, flags, null)
        { }

        public Program(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
                          int major, int minor, GraphicsContextFlags flags, IGraphicsContext sharedContext)
            : base(width, height, title, options,
                   mode == null ? GraphicsMode.Default : mode,
                   device == null ? DisplayDevice.Default : device)
        {
            try
            {
                glContext = new GraphicsContext(mode == null ? GraphicsMode.Default : mode, WindowInfo, major, minor, flags);
                glContext.MakeCurrent(WindowInfo);
                (glContext as IGraphicsContextInternal).LoadAll();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                base.Dispose();
                throw;
            }
        }

        #endregion

        #region --- Public Members ---

        #region Methods

        #region Dispose

        /// <summary>
        /// Disposes of the Program, releasing all resources consumed by it.
        /// </summary>
        public override void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                try
                {
                    if (glContext != null)
                    {
                        glContext.Dispose();
                        glContext = null;
                    }
                }
                finally
                {
                    base.Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        #region MakeCurrent

        /// <summary>
        /// Makes the GraphicsContext current on the calling thread.
        /// </summary>
        public void MakeCurrent()
        {
            EnsureUndisposed();
            Context.MakeCurrent(WindowInfo);
        }

        #endregion

        #region OnClose

        /// <summary>
        /// Called when the NativeWindow is about to close.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.CancelEventArgs" /> for this event.
        /// Set e.Cancel to true in order to stop the Program from closing.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                isExiting = true;
                OnUnloadInternal(EventArgs.Empty);
            }
        }


        #endregion

        #region OnLoad

        /// <summary>
        /// Called after an OpenGL context has been established, but before entering the main loop.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected virtual void OnLoad(EventArgs e)
        {
            if (Load != null) Load(this, e);
        }

        #endregion

        #region OnUnload

        /// <summary>
        /// Called after Program.Exit was called, but before destroying the OpenGL context.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected virtual void OnUnload(EventArgs e)
        {
            if (Unload != null) Unload(this, e);
        }

        #endregion

        public void SetVSync(bool enable)
        {
            GraphicsContext.CurrentContext.SwapInterval = enable ? 1 : 0;
            this.vsync = enable;
        }

        public void SetFramesPerSecond(double targetUpdatesPerSecond)
        {
            this.targetUpdateInterval = targetUpdatesPerSecond <= 0 ? 0 : 1 / targetUpdatesPerSecond;
        }

        public void Run()
        {
            Run(0.0, 0.0);
        }

        public void Run(double targetUpdatesPersecond)
        {
            Run(targetUpdatesPersecond, 0.0);
        }

        public void Run(double targetUpdatesPerSecond, double targetDrawsPerSecond, double maximumFrameTimeFactor = 3, bool dontOverrideFps = false)
        {
            EnsureUndisposed();

            Visible = true;   // Make sure the Program is visible.
            OnLoadInternal(EventArgs.Empty);
            OnResize(EventArgs.Empty);

            this.Context.MakeCurrent(null);

            var glThread = new Thread(() =>
                this.run(targetUpdatesPerSecond, targetDrawsPerSecond, maximumFrameTimeFactor, dontOverrideFps)
                );


            glThread.Start();

            this.eventHandleLoop();

            glThread.Abort();
        }

        private readonly ManualActionQueue uiQueue = new ManualActionQueue();

        public IActionQueue UIActionQueue { get { return this.uiQueue; } }

        private void eventHandleLoop()
        {
            while (this.Exists && !this.IsExiting)
            {
                this.ProcessEvents();
                this.uiQueue.ExecuteFor(TimeSpan.FromMilliseconds(2));
            }
        }

        private void run(double targetUpdatesPerSecond, double targetDrawsPerSecond, double maximumFrameTimeFactor = 3, bool dontOverrideFps = false)
        {
            this.Context.MakeCurrent(this.WindowInfo);

            double prevUpdateInterval = this.targetUpdateInterval;

            this.targetUpdateInterval = targetUpdatesPerSecond <= 0 ? 0 : 1 / targetUpdatesPerSecond;
            double targetRenderInterval = targetDrawsPerSecond <= 0 ? 0 : 1 / targetDrawsPerSecond;

            double maximumUpdateInterval = this.targetUpdateInterval == 0
                ? double.PositiveInfinity
                : this.targetUpdateInterval * maximumFrameTimeFactor;

            if (dontOverrideFps)
                this.targetUpdateInterval = prevUpdateInterval;

            UpdateEventArgs updateEventArgs = new UpdateEventArgs(0);

            double lastTimerTime = 0;

            double gameSeconds = 0;

            double nextTargetRenderTime = 0;

            var gameTimer = Stopwatch.StartNew();


            // main loop
            while (true)
            {
                double thisTimerTime = gameTimer.Elapsed.TotalSeconds;
                double elapsedSeconds = thisTimerTime - lastTimerTime;
                lastTimerTime = thisTimerTime;

                double updateSeconds = Math.Min(elapsedSeconds, maximumUpdateInterval);

                gameSeconds += updateSeconds;


                //this.ProcessEvents();
                if (this.Exists && !this.IsExiting)
                {
                    // update
                    updateEventArgs = new UpdateEventArgs(updateEventArgs, gameSeconds);

                    this.OnUpdate(updateEventArgs);

                    if (thisTimerTime >= nextTargetRenderTime)
                    {
                        // render
                        this.OnRender(updateEventArgs);
                        nextTargetRenderTime = thisTimerTime + targetRenderInterval;
                    }
                }
                else
                    return;

                if (!this.vsync)
                {
                    double timeAfterFrame = gameTimer.Elapsed.TotalSeconds;

                    double frameTime = timeAfterFrame - thisTimerTime;
                    double waitTime = this.targetUpdateInterval - frameTime;
                    if (waitTime > 0)
                        Thread.Sleep((int)(waitTime * 1000));
                }
            }
        }
        
        protected virtual void OnUpdate(UpdateEventArgs e) { }
        protected virtual void OnRender(UpdateEventArgs e) { }


        public void SwapBuffers()
        {
            EnsureUndisposed();
            this.Context.SwapBuffers();
        }

        public Bitmap GrabScreenshot()
        {
            var size = this.ClientSize;

            return this.GrabScreenshot(0, 0, size.Width, size.Height);
        }
        public Bitmap GrabScreenshot(int x, int y, int width, int height)
        {
            var bmp = new Bitmap(width, height);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.ReadPixels(x, y, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }

        public void SaveScreenshot(
            string path, string filename = "screenshot",
            bool saveAsPng = true, bool appendDate = true)
        {
            var size = this.ClientSize;

            this.SaveScreenshot(0, 0, size.Width, size.Height, path, filename, saveAsPng, appendDate);
        }

        public void SaveScreenshot(int x, int y, int width, int height,
            string path, string filename = "screenshot",
            bool saveAsPng = true, bool appendDate = true)
        {
            using (var bitmap = this.GrabScreenshot(x, y, width, height))
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var extension = "";

                // strip extension from file name
                if (saveAsPng)
                {
                    if (filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = filename.Substring(filename.Length - 4);
                        filename = filename.Substring(0, filename.Length - 4);
                    }
                    else
                    {
                        extension = ".png";
                    }
                }
                else
                {

                    if (filename.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = filename.Substring(filename.Length - 4);
                        filename = filename.Substring(0, filename.Length - 4);
                    }
                    else if (filename.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = filename.Substring(filename.Length - 5);
                        filename = filename.Substring(0, filename.Length - 5);
                    }
                    else
                    {
                        extension = ".jpg";
                    }
                }

                filename += appendDate
                    ? "_" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.ff") + extension
                    : extension;

                if (saveAsPng)
                {
                    bitmap.Save(path + "/" + filename, System.Drawing.Imaging.ImageFormat.Png);   
                }
                else
                {
                    var encoder = ImageCodecInfo.GetImageDecoders()
                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var parameters = new EncoderParameters(1);
                    parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

                    bitmap.Save(path + "/" + filename, encoder, parameters);
                }
            }
        }

        #region Properties

        #region Context

        /// <summary>
        /// Returns the opengl IGraphicsContext associated with the current Program.
        /// </summary>
        public IGraphicsContext Context
        {
            get
            {
                EnsureUndisposed();
                return glContext;
            }
        }

        #endregion

        #region IsExiting

        /// <summary>
        /// Gets a value indicating whether the shutdown sequence has been initiated
        /// for this window, by calling Program.Exit() or hitting the 'close' button.
        /// If this property is true, it is no longer safe to use any OpenTK.Input or
        /// OpenTK.Graphics.OpenGL functions or properties.
        /// </summary>
        public bool IsExiting
        {
            get
            {
                EnsureUndisposed();
                return isExiting;
            }
        }

        #endregion

        #region Joysticks
        
        /// <summary>
        /// Gets a readonly IList containing all available OpenTK.Input.JoystickDevices.
        /// </summary>
        public IList<JoystickDevice> Joysticks
        {
            get { return InputDriver.Joysticks; }
        }
        
        #endregion

        #region Keyboard

        /// <summary>
        /// Gets the primary Keyboard device, or null if no Keyboard exists.
        /// </summary>
        public KeyboardDevice Keyboard
        {
            get { return InputDriver.Keyboard.Count > 0 ? InputDriver.Keyboard[0] : null; }
        }

        #endregion

        #region Mouse

        /// <summary>
        /// Gets the primary Mouse device, or null if no Mouse exists.
        /// </summary>
        public MouseDevice Mouse
        {
            get { return InputDriver.Mouse.Count > 0 ? InputDriver.Mouse[0] : null; }
        }

        #endregion


        #region WindowState

        /// <summary>
        /// Gets or states the state of the NativeWindow.
        /// </summary>
        public override WindowState WindowState
        {
            get
            {
                return base.WindowState;
            }
            set
            {
                base.WindowState = value;
                Debug.Print("Updating Context after setting WindowState to {0}", value);

                if (Context != null)
                    Context.Update(WindowInfo);
            }
        }
        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs before the window is displayed for the first time.
        /// </summary>
        public event EventHandler<EventArgs> Load;

        /// <summary>
        /// Occurs when it is time to render a frame.
        /// </summary>
        public event EventHandler<FrameEventArgs> RenderFrame;

        /// <summary>
        /// Occurs before the window is destroyed.
        /// </summary>
        public event EventHandler<EventArgs> Unload;

        /// <summary>
        /// Occurs when it is time to update a frame.
        /// </summary>
        public event EventHandler<FrameEventArgs> UpdateFrame;

        #endregion

        #endregion

        #endregion

        #region --- Protected Members ---

        #region Dispose

        /// <summary>
        /// Override to add custom cleanup logic.
        /// </summary>
        /// <param name="manual">True, if this method was called by the application; false if this was called by the finalizer thread.</param>
        protected virtual void Dispose(bool manual) { }

        #endregion

        #region OnRenderFrame

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        /// <param name="e">Contains information necessary for frame rendering.</param>
        /// <remarks>
        /// Subscribe to the <see cref="RenderFrame"/> event instead of overriding this method.
        /// </remarks>
        protected virtual void OnRenderFrame(FrameEventArgs e)
        {
            if (RenderFrame != null) RenderFrame(this, e);
        }

        #endregion

        #region OnUpdateFrame

        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        /// <param name="e">Contains information necessary for frame updating.</param>
        /// <remarks>
        /// Subscribe to the <see cref="UpdateFrame"/> event instead of overriding this method.
        /// </remarks>
        protected virtual void OnUpdateFrame(FrameEventArgs e)
        {
            if (UpdateFrame != null) UpdateFrame(this, e);
        }

        #endregion

        #region OnWindowInfoChanged

        /// <summary>
        /// Called when the WindowInfo for this Program has changed.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected virtual void OnWindowInfoChanged(EventArgs e) { }

        #endregion

        #region OnResize

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            glContext.Update(base.WindowInfo);
        }

        #endregion

        #endregion

        #region --- Private Members ---

        #region OnLoadInternal

        private void OnLoadInternal(EventArgs e)
        {
            OnLoad(e);
        }

        #endregion

        #region OnRenderFrameInternal

        private void OnRenderFrameInternal(FrameEventArgs e) { if (Exists && !isExiting) OnRenderFrame(e); }

        #endregion

        #region OnUnloadInternal

        private void OnUnloadInternal(EventArgs e) { OnUnload(e); }

        #endregion

        #region OnUpdateFrameInternal

        private void OnUpdateFrameInternal(FrameEventArgs e) { if (Exists && !isExiting) OnUpdateFrame(e); }

        #endregion

        #region OnWindowInfoChangedInternal

        private void OnWindowInfoChangedInternal(EventArgs e)
        {
            glContext.MakeCurrent(WindowInfo);

            OnWindowInfoChanged(e);
        }

        #endregion

        #endregion
        
    }
}
