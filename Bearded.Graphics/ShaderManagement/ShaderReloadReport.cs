using System;
using System.Collections.Immutable;
using System.Linq;

namespace Bearded.Graphics.ShaderManagement
{
    public sealed class ShaderReloadReport
    {
        public static ShaderReloadReport NoChanges { get; }
            = new ShaderReloadReport(0, 0, ImmutableArray<Exception>.Empty);

        public bool TriedReloadingAnything => ReloadedAnything || ReloadExceptions.Any();

        public bool ReloadedAnything => ReloadedShaderCount != 0 || ReloadedProgramCount != 0;

        public int ReloadedShaderCount { get; }
        public int ReloadedProgramCount { get; }

        public ImmutableArray<Exception> ReloadExceptions { get; }

        public ShaderReloadReport(int reloadedShaderCount, int reloadedProgramCount, ImmutableArray<Exception> reloadExceptions)
        {
            ReloadExceptions = reloadExceptions;
            ReloadedShaderCount = reloadedShaderCount;
            ReloadedProgramCount = reloadedProgramCount;
        }
    }
}
