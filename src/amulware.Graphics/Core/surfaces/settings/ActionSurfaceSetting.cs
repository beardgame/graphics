using System;

namespace amulware.Graphics
{
    sealed public class ActionSurfaceSetting : SurfaceSetting
    {
        private readonly Action<ShaderProgram> set;
        private readonly Action<ShaderProgram> unset;

        public ActionSurfaceSetting(Action<ShaderProgram> set, Action<ShaderProgram> unset = null)
            : base(unset != null)
        {
            this.set = set;
            this.unset = unset;
        }

        public override void Set(ShaderProgram program)
        {
            this.set(program);
        }

        public override void UnSet(ShaderProgram program)
        {
            this.unset(program);
        }
    }
}
