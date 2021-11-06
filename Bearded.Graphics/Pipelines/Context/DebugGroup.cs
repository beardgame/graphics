using System;
using Bearded.Graphics.Debugging;
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
            KHRDebugExtension.Instance.PushDebugGroup(DebugSourceExternal.DebugSourceThirdParty, 0, name);
        }

        public void RestoreToStoredValue()
        {
            KHRDebugExtension.Instance.PopDebugGroup();
        }
    }
}
