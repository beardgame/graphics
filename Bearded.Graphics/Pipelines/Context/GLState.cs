using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using static Bearded.Graphics.Pipelines.Context.CullMode;

namespace Bearded.Graphics.Pipelines.Context
{
    public static class GLState
    {
        public static ColorMask ColorMask { get; private set; } = ColorMask.DrawAll;

        public static void SetColorMask(ColorMask mask)
        {
            ColorMask = mask;
            GL.ColorMask(
                mask.HasFlag(ColorMask.DrawRed),
                mask.HasFlag(ColorMask.DrawGreen),
                mask.HasFlag(ColorMask.DrawBlue),
                mask.HasFlag(ColorMask.DrawAll)
                );
        }

        public static int Framebuffer { get; private set; }

        public static void BindFramebuffer(int framebuffer)
        {
            Framebuffer = framebuffer;
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, framebuffer);
        }

        public static DepthMode DepthMode { get; private set; } = DepthMode.Disable;

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

            if (mode == BlendMode.Disable)
            {
                GL.Disable(EnableCap.Blend);
                return;
            }

            GL.Enable(EnableCap.Blend);

            var (src, dst, equation) = mode;

            GL.BlendFunc(src, dst);
            GL.BlendEquation(equation);
        }

        public static CullMode CullMode { get; private set; }

        public static void SetCullMode(CullMode cullMode)
        {
            CullMode = cullMode;

            if (cullMode == RenderAll)
            {
                GL.Disable(EnableCap.CullFace);
                return;
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(
                cullMode switch
                {
                    RenderAll => throw new InvalidOperationException(),
                    RenderFront => CullFaceMode.Back,
                    RenderBack => CullFaceMode.Front,
                    RenderNone => CullFaceMode.FrontAndBack,
                    _ =>  throw new ArgumentException($"Unsupported or unknown cull mode {cullMode}", nameof(cullMode)),
                }
                );
        }

        public static Rectangle Viewport { get; private set; }

        public static void SetViewport(Rectangle viewport)
        {
            Viewport = viewport;
            GL.Viewport(viewport);
        }

        public static ScissorRegion ScissorRegion { get; private set; }

        public static void SetScissorRegion(ScissorRegion region)
        {
            ScissorRegion = region;

            switch (region.Rectangle)
            {
                case null:
                    GL.Disable(EnableCap.ScissorTest);
                    return;
                case { } r:
                    GL.Enable(EnableCap.ScissorTest);
                    GL.Scissor(r.X, r.Y, r.Width, r.Height);
                    break;
            }
        }
    }
}
