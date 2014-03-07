using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using amulware.Graphics.Serialization.JsonNet;
using Newtonsoft.Json;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
    {
        private readonly string name;
        private readonly SkeletonTemplate<TBoneAttributes> skeleton;
        private readonly ReadOnlyCollection<Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>> keyframes;
        private readonly Dictionary<string, Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>> keyframeDictionary;
        private readonly ReadOnlyCollection<AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>> sequences;
        private readonly Dictionary<string, AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>> sequenceDictionary;

        internal AnimationTemplate(AnimationTemplateJsonRepresentation<TKeyframeParameters, TBoneAttributes> json)
        {
            this.name = json.Name;
            this.skeleton = new SkeletonTemplate<TBoneAttributes>(json.Skeleton);

            this.keyframes = json.Keyframes == null ? new List<Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>>().AsReadOnly()
                : json.Keyframes.Select(f => new Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>(f, this.skeleton)).ToList().AsReadOnly();

            this.keyframeDictionary = this.keyframes.ToDictionary(f => f.Name);

            this.sequences = json.Sequences == null
                ? new List<AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>>().AsReadOnly()
                : json.Sequences.Select(
                    s => new AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>(s, this.keyframeDictionary))
                        .ToList().AsReadOnly();

            this.sequenceDictionary = this.sequences.ToDictionary(s => s.Name);
        }

        public string Name { get { return this.name; } }
        public SkeletonTemplate<TBoneAttributes> Skeleton { get { return this.skeleton; } }

        public ReadOnlyCollection<Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>> Keyframes
        {
            get { return this.keyframes; }
        }

        public ReadOnlyCollection<AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>> Sequences
        {
            get { return this.sequences; }
        }

        public AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> GetSequence(string name)
        {
            AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> s;
            this.sequenceDictionary.TryGetValue(name, out s);
            return s;
        }

        public Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> GetKeyFrame(string name)
        {
            Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> f;
            this.keyframeDictionary.TryGetValue(name, out f);
            return f;
        }

        public static AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> FromJsonFile(string filename)
        {
            return AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>.FromJsonTextReader(
                File.OpenText(filename));
        }

        public static AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> FromJsonTextReader(
            TextReader textreader)
        {
            var serialiser = new JsonSerializer().ConfigureForGraphics();

            return new AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>(
                serialiser.Deserialize<AnimationTemplateJsonRepresentation<TKeyframeParameters, TBoneAttributes>>(
                new JsonTextReader(textreader)));
        }

        public AnimationSystem<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>
            Instantiate<TBoneTransformation>(string baseFrameName = "base")
            where TBoneTransformation :
                IBoneTransformation<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>, new()
        {
            return new AnimationSystem<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>(
                this, baseFrameName);
        }
    }
}
