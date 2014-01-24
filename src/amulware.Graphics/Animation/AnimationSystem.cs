using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSystem<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
        where TBoneTransformation : IBoneTransformation<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>, new()
    {
        private AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> template;

        private List<AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes>> activeSeqences
            = new List<AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes>>();

        private ReadOnlyCollection<Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>> skeleton;

        private TBoneParameters[] baseParameters;
        private TBoneParameters[] parameters;

        private int[] rootIndices;

        public TKeyframeParameters RootParameters { get; set; }

        public AnimationSystem(
            AnimationTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> template,
            string baseFrameName = "base")
        {
            this.template = template;

            var skeleton = new List<Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>>(template.Skeleton.Bones.Count);
            var rootIndices = new List<int>();
            foreach (var b in template.Skeleton.Bones)
            {
                skeleton.Add(
                    new Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>
                        (b.Parent == null ? null : skeleton[b.Parent.Id], b)
                    );
                if (b.Parent == null)
                    rootIndices.Add(b.Id);
            }
            this.skeleton = skeleton.AsReadOnly();
            this.rootIndices = rootIndices.ToArray();

            this.baseParameters = new TBoneParameters[this.skeleton.Count];
            this.parameters = new TBoneParameters[this.skeleton.Count];

            for (int i = 0; i < this.baseParameters.Length; i++)
            {
                TBoneParameters parameter = this.baseParameters[i];
                parameter.SetToDefault();
                this.baseParameters[i] = parameter;
            }

            Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> baseFrame;
            if (baseFrameName != null && (baseFrame = template.GetKeyFrame(baseFrameName)) != null)
            {
                baseFrame.ApplyTo(this.baseParameters, 1);
            }

            this.RootParameters = default(TKeyframeParameters);
        }

        public ReadOnlyCollection<Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>>
            Skeleton { get { return this.skeleton; } }

        public bool Start(string sequenceName, AnimationSequence.Mode mode, bool startPaused = false)
        {
            AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes> s;
            return this.Start(sequenceName, mode, startPaused, out s);
        }

        public bool Start(string sequenceName, AnimationSequence.Mode mode, bool startPaused,
            out AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes> sequence)
        {
            var template = this.template.GetSequence(sequenceName);
            if (template == null)
            {
                sequence = null;
                return false;
            }
            sequence = new AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes>(template, mode,
                startPaused ? AnimationSequence.PlayState.Waiting : AnimationSequence.PlayState.Playing);
            this.activeSeqences.Add(sequence);
            return true;
        }

        public bool Stop(AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes> sequence)
        {
            return this.activeSeqences.Remove(sequence);
        }

        public void StopAll()
        {
            this.activeSeqences.Clear();
        }

        public void Update(float advanceTime)
        {
            this.baseParameters.CopyTo(this.parameters, 0);

            for (int i = 0; i < this.rootIndices.Length; i++)
            {
                this.parameters[this.rootIndices[i]].Add(this.RootParameters, 1);
            }

            foreach (var sequence in this.activeSeqences)
            {
                sequence.AdvanceTime(advanceTime);
                sequence.ApplyTo(this.parameters);
            }

            this.activeSeqences.RemoveAll(s => s.Stopped);

            for (int i = 0; i < this.parameters.Length; i++)
            {
                this.skeleton[i].UpdateParameters(this.parameters[i]);
            }

        }
    }
}
