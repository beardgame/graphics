using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public abstract class SurfaceSetting
    {
        public readonly bool NeedsUnsetting = false;

        protected SurfaceSetting() { }

        protected SurfaceSetting(bool needsUnsetting)
        {
            this.NeedsUnsetting = needsUnsetting;
        }

        public abstract void Set(ShaderProgram program);

        public virtual void UnSet(ShaderProgram program) { }
    }
}
