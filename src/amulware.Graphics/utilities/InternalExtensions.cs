
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.utilities
{
    internal static class InternalExtensions
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }


        private static bool checkedForLegacyMode;
        private static bool isInLegacyMode;

        internal static bool IsInLegacyMode
        {
            get
            {
                if (!InternalExtensions.checkedForLegacyMode)
                {
                    InternalExtensions.isInLegacyMode = InternalExtensions.getLegacy();
                    InternalExtensions.checkedForLegacyMode = true;
                }
                return InternalExtensions.isInLegacyMode;
            }
        }

        private static bool getLegacy()
        {
            string version = GL.GetString(StringName.Version);
            string shader = GL.GetString(StringName.ShadingLanguageVersion);

            var vs = version.Split(' ')[0].Split('.');
            var ss = shader.Split(' ')[0].Split('.');

            int vMajor = int.Parse(vs[0]);
            int vMinor = int.Parse(vs[1]);

            int sMajor = int.Parse(ss[0]);
            int sMinor = int.Parse(ss[1]);

            if (vMajor < 3)
                return true;
            if (vMajor < 2 && vMinor < 3)
                return true;

            return false;

        }
    }
}
