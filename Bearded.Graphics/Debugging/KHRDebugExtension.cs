using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Debugging
{
    public abstract class KHRDebugExtension
    {
        private static KHRDebugExtension? instance;
        public static KHRDebugExtension Instance => instance ??= createInstanceWithAvailableFunctionality();

        private KHRDebugExtension()
        {
        }

        private static KHRDebugExtension createInstanceWithAvailableFunctionality()
        {
            if (khrDebugExtensionIsAvailable())
                return new ActualImplementation();
            return new DummyImplementation();
        }

        private static bool khrDebugExtensionIsAvailable()
        {
            var count = GL.GetInteger(GetPName.NumExtensions);

            return Enumerable.Range(0, count)
                .Select(i => GL.GetString(StringNameIndexed.Extensions, i))
                .Contains("GL_KHR_debug");
        }

        public virtual void SetObjectLabel(ObjectLabelIdentifier identifier, int name, string label) { }
        public virtual void PushDebugGroup(DebugSourceExternal source, int id, string name) { }
        public virtual void PopDebugGroup() { }

        private sealed class ActualImplementation : KHRDebugExtension
        {
            public override void SetObjectLabel(ObjectLabelIdentifier identifier, int name, string label)
            {
                GL.ObjectLabel(identifier, name, label.Length, label);
            }

            public override void PushDebugGroup(DebugSourceExternal source, int id, string name)
            {
                GL.PushDebugGroup(source, id, name.Length, name);
            }

            public override void PopDebugGroup()
            {
                GL.PopDebugGroup();
            }
        }

        private sealed class DummyImplementation : KHRDebugExtension
        {
        }
    }
}
