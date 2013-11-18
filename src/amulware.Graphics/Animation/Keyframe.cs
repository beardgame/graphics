using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using amulware.Graphics.utilities;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace amulware.Graphics.Animation
{
    sealed public class Keyframe
    {
        private readonly string name;
        private readonly ReadOnlyCollection<KeyframeData> data;

        internal Keyframe(KeyframeJsonRepresentation frame, SkeletonTemplate skeleton)
        {
            if (string.IsNullOrEmpty(frame.Name))
                throw new InvalidDataException("Keyframe must have name.");
            this.name = frame.Name;

            if (frame.Data == null)
            {
                this.data = new List<KeyframeData>().AsReadOnly();
                return;
            }

            this.data = frame.Data.Select(d => new KeyframeData(d, skeleton)).ToList().AsReadOnly();
        }

        public string Name
        {
            get { return this.name; }
        }

        public ReadOnlyCollection<KeyframeData> Data { get { return this.data; } }

        public void ApplyTo(BoneParameters[] parameters, float weight)
        {
            foreach (var d in this.data)
                parameters[d.Bone.Id].Add(d, weight);
        }
    }
}
