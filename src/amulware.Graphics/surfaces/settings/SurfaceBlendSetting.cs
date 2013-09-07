using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This immutable class represents a blend function surface setting.
    /// </summary>
    public class SurfaceBlendSetting : SurfaceSetting
    {
        private BlendingFactorSrc srcBlend;
        private BlendingFactorDest destBlend;
        private BlendEquationMode equation;

        /// <summary>Default 'Alpha' blend function</summary>
        public static readonly SurfaceBlendSetting Alpha = new SurfaceBlendSetting(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendEquationMode.FuncAdd);
        /// <summary>Default 'Add' blend function</summary>
        public static readonly SurfaceBlendSetting Add = new SurfaceBlendSetting(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendEquationMode.FuncAdd);
        /// <summary>Default 'Substract' blend function</summary>
        public static readonly SurfaceBlendSetting Substract = new SurfaceBlendSetting(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendEquationMode.FuncReverseSubtract);
        /// <summary>Default 'Multiply' blend function</summary>
        public static readonly SurfaceBlendSetting Multiply = new SurfaceBlendSetting(BlendingFactorSrc.Zero, BlendingFactorDest.SrcColor, BlendEquationMode.FuncAdd);
        /// <summary>Default 'Minimum' blend function</summary>
        public static readonly SurfaceBlendSetting Min = new SurfaceBlendSetting(BlendingFactorSrc.One, BlendingFactorDest.One, BlendEquationMode.Min);
        /// <summary>Default 'Maximum' blend function</summary>
        public static readonly SurfaceBlendSetting Max = new SurfaceBlendSetting(BlendingFactorSrc.One, BlendingFactorDest.One, BlendEquationMode.Max);

        /// <summary>
        /// Initializes a new custom instance of the <see cref="SurfaceBlendSetting"/> class.
        /// </summary>
        /// <param name="src">The <see cref="BlendingFactorSrc"/>.</param>
        /// <param name="dest">The <see cref="BlendingFactorDest"/>.</param>
        /// <param name="equation">The <see cref="BlendEquationMode"/>.</param>
        public SurfaceBlendSetting(BlendingFactorSrc src, BlendingFactorDest dest, BlendEquationMode equation)
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
