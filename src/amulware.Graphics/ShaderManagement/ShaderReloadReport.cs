using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ShaderReloadReport
    {
        public static ShaderReloadReport NoChanges { get; }
            = new ShaderReloadReport(0, 0, new List<Exception>().AsReadOnly());
        
        public bool TriedReloadingAnything => ReloadedAnything || ReloadExceptions.Any();

        public bool ReloadedAnything => ReloadedShaderCount != 0 || ReloadedProgramCount != 0;
        
        public int ReloadedShaderCount { get; }
        public int ReloadedProgramCount { get; }
        
        public ReadOnlyCollection<Exception> ReloadExceptions { get; }
        
        public ShaderReloadReport(int reloadedShaderCount, int reloadedProgramCount, ReadOnlyCollection<Exception> reloadExceptions)
        {
            ReloadExceptions = reloadExceptions;
            ReloadedShaderCount = reloadedShaderCount;
            ReloadedProgramCount = reloadedProgramCount;
        }
    }
}