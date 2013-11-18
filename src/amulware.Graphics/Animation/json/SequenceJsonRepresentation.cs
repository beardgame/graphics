using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    internal class SequenceJsonRepresentation
    {
        public string Name { get; set; }
        public List<TransitionJsonRepresentation> Transitions { get; set; }
    }
}
