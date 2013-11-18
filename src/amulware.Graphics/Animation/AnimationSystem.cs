using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using amulware.Graphics.utilities;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSystem
    {
        private AnimationTemplate template;

        private List<AnimationSeqence> activeSeqences = new List<AnimationSeqence>();

        private ReadOnlyCollection<Bone> skeleton;

        private BoneParameters[] baseParameters;
        private BoneParameters[] parameters;

        public AnimationSystem(AnimationTemplate template, string baseFrameName = "base")
        {
            this.template = template;

            var skeleton = new List<Bone>(template.Skeleton.Bones.Count);
            foreach (var b in template.Skeleton.Bones)
            {
                skeleton.Add(new Bone(b.Parent == null ? null : skeleton[b.Parent.Id]));
            }
            this.skeleton = skeleton.AsReadOnly();

            this.baseParameters = new BoneParameters[this.skeleton.Count];
            this.parameters = new BoneParameters[this.skeleton.Count];
            Keyframe baseFrame;
            if (baseFrameName != null && (baseFrame = template.GetKeyFrame("base")) != null)
            {
                foreach(var d in baseFrame.Data)
                    this.baseParameters[d.Bone.Id] = new BoneParameters(d);
            }
        }

        public ReadOnlyCollection<Bone> Skeleton { get { return this.skeleton; } }

        public bool Start(string sequenceName, AnimationSeqence.Mode mode, bool startPaused = false)
        {
            AnimationSeqence s;
            return this.Start(sequenceName, mode, startPaused, out s);
        }

        public bool Start(string sequenceName, AnimationSeqence.Mode mode, bool startPaused, out AnimationSeqence sequence)
        {
            var template = this.template.GetSequence(sequenceName);
            if (template == null)
            {
                sequence = null;
                return false;
            }
            sequence = new AnimationSeqence(template, mode,
                startPaused ? AnimationSeqence.PlayState.Waiting : AnimationSeqence.PlayState.Playing);
            this.activeSeqences.Add(sequence);
            return true;
        }

        public bool Stop(AnimationSeqence sequence)
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

            foreach (var sequence in this.activeSeqences)
            {
                sequence.AdvanceTime(advanceTime);
                sequence.ApplyTo(this.parameters);
            }

            this.activeSeqences.RemoveAll(s => s.Stopped);

            for (int i = 0; i < this.parameters.Length; i++)
            {
                this.skeleton[i].SetParameters(this.parameters[i]);
                this.skeleton[i].Recalculate();
            }

        }
    }
}
