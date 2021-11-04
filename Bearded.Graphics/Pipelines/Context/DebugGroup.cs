using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Pipelines.Context
{
    sealed class DebugGroup<TState> : IContextChange<TState>
    {
        private readonly Func<TState, string> getName;

        public DebugGroup(string name)
            : this(_ => name)
        {
        }

        public DebugGroup(Func<TState,string> getName)
        {
            this.getName = getName;
        }

        public void StoreCurrentValueAndApplyChange(TState state)
        {
            var name = getName(state);
            GL.PushDebugGroup(DebugSourceExternal.DebugSourceThirdParty, 0, name.Length, name);
        }

        public void RestoreToStoredValue()
        {
            GL.PopDebugGroup();
        }
    }
}
