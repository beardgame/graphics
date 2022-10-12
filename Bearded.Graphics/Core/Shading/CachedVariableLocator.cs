using System;
using System.Collections.Generic;

namespace Bearded.Graphics.Shading
{
    internal sealed class CachedVariableLocator
    {
        private readonly Func<string, int> locateVariable;
        private readonly Dictionary<string, int> variableLocations = new();

        public CachedVariableLocator(Func<string, int> locateVariable)
        {
            this.locateVariable = locateVariable;
        }

        public int GetVariableLocation(string name)
        {
            if (!variableLocations.TryGetValue(name, out var i))
            {
                i = locateVariable(name);
                variableLocations.Add(name, i);
            }

            return i;
        }
    }
}
