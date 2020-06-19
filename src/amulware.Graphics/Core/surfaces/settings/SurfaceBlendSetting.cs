using OpenToolkit.Graphics.OpenGL;
using static OpenToolkit.Graphics.OpenGL.BlendEquationMode;
using static OpenToolkit.Graphics.OpenGL.BlendingFactor;

namespace amulware.Graphics
{
    /// <summary>
    /// This immutable class represents a blend function surface setting.
    /// </summary>
    public class SurfaceBlendSetting : SurfaceSetting
    {
        private readonly BlendingFactor srcBlend;
        private readonly BlendingFactor destBlend;
        private readonly BlendEquationMode equation;

        /// <summary>Default 'Alpha' blend function</summary>
        public static readonly SurfaceBlendSetting Alpha = new SurfaceBlendSetting(SrcAlpha, OneMinusSrcAlpha, FuncAdd);
        /// <summary>Default 'Pre-multiplied Alpha' blend function</summary>
        public static readonly SurfaceBlendSetting PremultipliedAlpha = new SurfaceBlendSetting(One, OneMinusSrcAlpha, FuncAdd);
        /// <summary>Default 'Add' blend function</summary>
        public static readonly SurfaceBlendSetting Add = new SurfaceBlendSetting(SrcAlpha, One, FuncAdd);
        /// <summary>Default 'Substract' blend function</summary>
        public static readonly SurfaceBlendSetting Substract = new SurfaceBlendSetting(SrcAlpha, One, FuncReverseSubtract);
        /// <summary>Default 'Multiply' blend function</summary>
        public static readonly SurfaceBlendSetting Multiply = new SurfaceBlendSetting(Zero, SrcColor, FuncAdd);
        /// <summary>Default 'Minimum' blend function</summary>
        public static readonly SurfaceBlendSetting Min = new SurfaceBlendSetting(One, One, BlendEquationMode.Min);
        /// <summary>Default 'Maximum' blend function</summary>
        public static readonly SurfaceBlendSetting Max = new SurfaceBlendSetting(One, One, BlendEquationMode.Max);

        /// <summary>
        /// Initializes a new custom instance of the <see cref="SurfaceBlendSetting"/> class.
        /// </summary>
        /// <param name="src">The <see cref="BlendingFactorSrc"/>.</param>
        /// <param name="dest">The <see cref="BlendingFactorDest"/>.</param>
        /// <param name="equation">The <see cref="BlendEquationMode"/>.</param>
        public SurfaceBlendSetting(BlendingFactor src, BlendingFactor dest, BlendEquationMode equation)
            : base(true)
        {
            this.srcBlend = src;
            this.destBlend = dest;
            this.equation = equation;
        }

        /// <summary>
        /// Enables blending and sets the blend function for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(this.srcBlend, this.destBlend);
            GL.BlendEquation(this.equation);
        }

        /// <summary>
        /// Disables blending after draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void UnSet(ShaderProgram program)
        {
            GL.Disable(EnableCap.Blend);
        }
    }
}
