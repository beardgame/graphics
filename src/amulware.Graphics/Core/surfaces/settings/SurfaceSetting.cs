using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Base class for all surface settings.
    /// </summary>
    public abstract class SurfaceSetting
    {
        /// <summary>
        /// Wether this setting needs to 'unset' itself after the draw call.
        /// </summary>
        public readonly bool NeedsUnsetting = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceSetting"/> class.
        /// </summary>
        /// <remarks>Use this constructor only if the setting does not need to be unset after the draw call.</remarks>
        protected SurfaceSetting() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceSetting"/> class.
        /// </summary>
        /// <param name="needsUnsetting">if set to <c>true</c> makes the setting unset itself after the draw call.</param>
        /// <remarks>Make sure to set needsUnsetting to true, if this setting needs to be unset after the draw call.</remarks>
        protected SurfaceSetting(bool needsUnsetting)
        {
            this.NeedsUnsetting = needsUnsetting;
        }

        /// <summary>
        /// Sets the setting for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public abstract void Set(ShaderProgram program);

        /// <summary>
        /// Unsets the setting for a shader program. Is called after the draw call, if needsUnsetting was set to true in constructor.
        /// </summary>
        /// <param name="program">The program.</param>
        public virtual void UnSet(ShaderProgram program) { }
    }
}
