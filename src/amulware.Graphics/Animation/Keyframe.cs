using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace amulware.Graphics.Animation
{
    sealed public class Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
    {
        private readonly string name;
        private readonly ReadOnlyCollection<KeyframeData<TKeyframeParameters, TBoneAttributes>> data;

        internal Keyframe(KeyframeJsonRepresentation<TKeyframeParameters> frame, SkeletonTemplate<TBoneAttributes> skeleton)
        {
            if (string.IsNullOrEmpty(frame.Name))
                throw new InvalidDataException("Keyframe must have name.");
            this.name = frame.Name;

            if (frame.Data == null)
            {
                this.data = new List<KeyframeData<TKeyframeParameters, TBoneAttributes>>().AsReadOnly();
                return;
            }

            this.data = frame.Data.Select(
                d => new KeyframeData<TKeyframeParameters, TBoneAttributes>(d, skeleton))
                .ToList().AsReadOnly();
        }

        public string Name
        {
            get { return this.name; }
        }

        public ReadOnlyCollection<KeyframeData<TKeyframeParameters, TBoneAttributes>> Data { get { return this.data; } }

        public void ApplyTo(TBoneParameters[] parameters, float weight)
        {
            foreach (var d in this.data)
                parameters[d.Bone.Id].Add(d.Parameters, weight);
        }
    }
}
