using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class SurfaceDepthMaskSetting : SurfaceSetting
    {
        virtual public bool MaskDepth { get { return this.maskDepth; } set { this.maskDepth = value; } }

        private bool maskDepth;

        private class StaticSurfaceDepthMaskSetting : SurfaceDepthMaskSetting
        {
            public StaticSurfaceDepthMaskSetting(bool maskDepth)
                : base(maskDepth) { }

            public override bool MaskDepth
            {
                get { return base.MaskDepth; }
                set { return; }
            }
        }

        public static readonly SurfaceDepthMaskSetting DontMask = new StaticSurfaceDepthMaskSetting(false);

        public SurfaceDepthMaskSetting(bool maskDepth = false)
            : base(true)
        {
            this.MaskDepth = maskDepth;
        }

        public override void Set(ShaderProgram program)
        {
            GL.DepthMask(this.MaskDepth);
        }

        public override void UnSet(ShaderProgram program)
        {
            GL.DepthMask(true);
        }
    }
}
