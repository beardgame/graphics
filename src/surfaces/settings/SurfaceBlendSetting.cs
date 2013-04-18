using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public enum SurfaceBlendMode
    {
        Alpha,
        Add,
        Substract,
        Multiply,
        Min,
        Max
    }

    public class SurfaceBlendSetting : SurfaceSetting
    {
        private BlendingFactorSrc srcBlend;
        private BlendingFactorDest destBlend;
        private BlendEquationMode equation;

        static private Dictionary<SurfaceBlendMode, Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>> blendModes =
            new Dictionary<SurfaceBlendMode, Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>>()
            {
                {SurfaceBlendMode.Alpha, new Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>(
                    BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendEquationMode.FuncAdd)},

                {SurfaceBlendMode.Add, new Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>(
                    BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendEquationMode.FuncAdd)},

                {SurfaceBlendMode.Substract, new Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>(
                    BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendEquationMode.FuncReverseSubtract)},

                {SurfaceBlendMode.Multiply, new Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>(
                    BlendingFactorSrc.Zero, BlendingFactorDest.SrcColor, BlendEquationMode.FuncAdd)},

                {SurfaceBlendMode.Min, new Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>(
                    BlendingFactorSrc.One, BlendingFactorDest.One, BlendEquationMode.Min)},

                {SurfaceBlendMode.Max, new Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode>(
                    BlendingFactorSrc.One, BlendingFactorDest.One, BlendEquationMode.Max)}
            };


        private class StaticSurfaceBlendSetting : SurfaceBlendSetting
        {
            public StaticSurfaceBlendSetting(SurfaceBlendMode mode)
                : base(mode) { }

            public override SurfaceBlendMode BlendMode { set { return; } }
        }

        public static readonly SurfaceBlendSetting Alpha = new StaticSurfaceBlendSetting(SurfaceBlendMode.Alpha);
        public static readonly SurfaceBlendSetting Add = new StaticSurfaceBlendSetting(SurfaceBlendMode.Add);
        public static readonly SurfaceBlendSetting Substract = new StaticSurfaceBlendSetting(SurfaceBlendMode.Substract);
        public static readonly SurfaceBlendSetting Multiply = new StaticSurfaceBlendSetting(SurfaceBlendMode.Multiply);
        public static readonly SurfaceBlendSetting Min = new StaticSurfaceBlendSetting(SurfaceBlendMode.Min);
        public static readonly SurfaceBlendSetting Max = new StaticSurfaceBlendSetting(SurfaceBlendMode.Max);

        public SurfaceBlendSetting(SurfaceBlendMode mode)
            : base(true)
        {
            this.setBlendMode(mode);
        }

        public SurfaceBlendSetting(BlendingFactorSrc src, BlendingFactorDest dest, BlendEquationMode equation)
            : base(true)
        {
            this.srcBlend = src;
            this.destBlend = dest;
            this.equation = equation;
        }

        private void setBlendMode(SurfaceBlendMode mode)
        {
            Tuple<BlendingFactorSrc, BlendingFactorDest, BlendEquationMode> blendModes = SurfaceBlendSetting.blendModes[mode];
            this.srcBlend = blendModes.Item1;
            this.destBlend = blendModes.Item2;
            this.equation = blendModes.Item3;
        }

        virtual public SurfaceBlendMode BlendMode
        {
            set
            {
                this.setBlendMode(value);
            }
        }


        public override void Set(ShaderProgram program)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(this.srcBlend, this.destBlend);
            GL.BlendEquation(this.equation);
        }

        public override void UnSet(ShaderProgram program)
        {
            GL.Disable(EnableCap.Blend);
        }
    }
}
