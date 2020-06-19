using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This immutable class represents a depth mask surface setting.
    /// </summary>
    public class SurfaceDepthMaskSetting : SurfaceSetting
    {
        private bool maskDepth;

        /// <summary>Default 'Dont Mask' masking setting</summary>
        public static readonly SurfaceDepthMaskSetting DontMask = new SurfaceDepthMaskSetting(false);

        private SurfaceDepthMaskSetting(bool maskDepth = false)
            : base(true)
        {
            this.maskDepth = maskDepth;
        }

        /// <summary>
        /// Sets the depth masking setting for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.DepthMask(this.maskDepth);
        }

        /// <summary>
        /// Sets depth masking to default(enabled) after draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void UnSet(ShaderProgram program)
        {
            GL.DepthMask(true);
        }
    }
}
