using System;
using OpenToolkit.Graphics.OpenGL;
using static OpenToolkit.Graphics.OpenGL.BlendEquationMode;
using static OpenToolkit.Graphics.OpenGL.BlendingFactor;

namespace amulware.Graphics.Pipelines.Context
{
    public enum BlendMode
    {
        None = 0,
        Alpha,
        Add,
        Subtract,
        Multiply,
        Premultiplied,
        Min,
        Max,
    }

    public static class GLState
    {
        public static int Framebuffer { get; private set; }

        public static void BindFramebuffer(int framebuffer)
        {
            Framebuffer = framebuffer;
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, framebuffer);
        }

        public static DepthMode DepthMode { get; private set; }

        public static void SetDepthMode(DepthMode mode)
        {
            DepthMode = mode;

            if (mode == DepthMode.Disable)
            {
                GL.Disable(EnableCap.DepthTest);
                return;
            }
            GL.Enable(EnableCap.DepthTest);

            GL.DepthMask(mode.Write);
            GL.DepthFunc(mode.TestFunction);
        }

        public static BlendMode BlendMode { get; private set; }

        public static void SetBlendMode(BlendMode mode)
        {
            BlendMode = mode;

            if (mode == BlendMode.None)
            {
                GL.Disable(EnableCap.Blend);
                return;
            }

            GL.Enable(EnableCap.Blend);

            // TODO: refactor to work similarly to depth mode to make more flexible
            var (src, dst, equation) = mode switch
            {
                BlendMode.Alpha => (SrcAlpha, OneMinusSrcAlpha, FuncAdd),
                BlendMode.Add => (SrcAlpha, One, FuncAdd),
                BlendMode.Subtract => (SrcAlpha, One, FuncReverseSubtract),
                BlendMode.Multiply => (Zero, SrcColor, FuncAdd),
                BlendMode.Premultiplied => (One, OneMinusSrcAlpha, FuncAdd),
                BlendMode.Min => (One, One, Min),
                BlendMode.Max => (One, One, Max),
                BlendMode.None => throw new InvalidOperationException(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

            GL.BlendFunc(src, dst);
            GL.BlendEquation(equation);
        }
    }
}
