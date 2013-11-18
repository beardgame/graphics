using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using amulware.Graphics.Serialization.JsonNet;
using Newtonsoft.Json;
using OpenTK.Input;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationTemplate
    {
        private readonly string name;
        private readonly SkeletonTemplate skeleton;
        private readonly ReadOnlyCollection<Keyframe> keyframes;
        private readonly Dictionary<string, Keyframe> keyframeDictionary;
        private readonly ReadOnlyCollection<AnimationSequenceTemplate> sequences;
        private readonly Dictionary<string, AnimationSequenceTemplate> sequenceDictionary;

        internal AnimationTemplate(AnimationTemplateJsonRepresentation json)
        {
            this.name = json.Name;
            this.skeleton = new SkeletonTemplate(json.Skeleton);

            this.keyframes = json.Keyframes == null ? new List<Keyframe>().AsReadOnly()
                : json.Keyframes.Select(f => new Keyframe(f, this.skeleton)).ToList().AsReadOnly();

            this.keyframeDictionary = this.Keyframes.ToDictionary(f => f.Name);

            this.sequences = json.Sequences == null ? new List<AnimationSequenceTemplate>().AsReadOnly()
                : json.Sequences.Select(s => new AnimationSequenceTemplate(s, this.keyframeDictionary))
                    .ToList().AsReadOnly();

            this.sequenceDictionary = this.Sequences.ToDictionary(s => s.Name);
        }

        public string Name { get { return this.name; } }
        public SkeletonTemplate Skeleton { get { return this.skeleton; } }

        public ReadOnlyCollection<Keyframe> Keyframes
        {
            get { return this.keyframes; }
        }

        public ReadOnlyCollection<AnimationSequenceTemplate> Sequences
        {
            get { return this.sequences; }
        }

        public AnimationSequenceTemplate GetSequence(string name)
        {
            AnimationSequenceTemplate s;
            this.sequenceDictionary.TryGetValue(name, out s);
            return s;
        }

        public Keyframe GetKeyFrame(string name)
        {
            Keyframe f;
            this.keyframeDictionary.TryGetValue(name, out f);
            return f;
        }
        
        public static AnimationTemplate FromJsonFile(string filename)
        {
            return new AnimationTemplate(JsonConvert.DeserializeObject<AnimationTemplateJsonRepresentation>(
                File.ReadAllText(filename),
                new JsonSerializerSettings().ConfigureForGraphics()
                ));
        }
    }
}
