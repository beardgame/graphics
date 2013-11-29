using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using amulware.Graphics.utilities;
using OpenTK;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSystem
    {
        private AnimationTemplate template;

        private List<AnimationSeqence> activeSeqences = new List<AnimationSeqence>();

        private ReadOnlyCollection<Bone> skeleton;

        private BoneParameters[] baseParameters;
        private BoneParameters[] parameters;

        private int[] rootIndices;

        public BoneParameters RootParameters { get; set; }

        public AnimationSystem(AnimationTemplate template, string baseFrameName = "base")
        {
            this.template = template;

            var skeleton = new List<Bone>(template.Skeleton.Bones.Count);
            var rootIndices = new List<int>();
            foreach (var b in template.Skeleton.Bones)
            {
                skeleton.Add(new Bone(b.Parent == null ? null : skeleton[b.Parent.Id], b.Sprite));
                if (b.Parent == null)
                    rootIndices.Add(b.Id);
            }
            this.skeleton = skeleton.AsReadOnly();
            this.rootIndices = rootIndices.ToArray();

            this.baseParameters = new BoneParameters[this.skeleton.Count];
            this.parameters = new BoneParameters[this.skeleton.Count];
            for (int i = 0; i < this.parameters.Length; i++)
            {
                this.baseParameters[i].Scale = 1;
            }
            Keyframe baseFrame;
            if (baseFrameName != null && (baseFrame = template.GetKeyFrame("base")) != null)
            {
                foreach(var d in baseFrame.Data)
                    this.baseParameters[d.Bone.Id] = new BoneParameters(d);
            }

            this.RootParameters = new BoneParameters(Vector2.Zero, 0, 1);
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

            for (int i = 0; i < this.rootIndices.Length; i++)
            {
                this.parameters[this.rootIndices[i]].Add(this.RootParameters);
            }

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
