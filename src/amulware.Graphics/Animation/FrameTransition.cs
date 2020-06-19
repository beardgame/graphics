using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace amulware.Graphics.Animation
{
    internal static class FrameTransition
    {
        internal enum TransitionType
        {
            Linear = 0,
            Smooth = 1
        }
    }

    sealed public class FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
    {

        public float StartTime { get; private set; }
        public float EndTime { get; private set; }
        public Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> StartFrame { get; private set; }
        public Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> EndFrame { get; private set; }
        public float Duration { get; private set; }
        public float Delay { get; private set; }
        public float DelayEnd { get; private set; }
        internal FrameTransition.TransitionType Transition { get; private set; }

        private readonly KeyframeData<TKeyframeParameters, TBoneAttributes>[] startOnlyData;
        private readonly KeyframeData<TKeyframeParameters, TBoneAttributes>[] startSharedData;
        private readonly KeyframeData<TKeyframeParameters, TBoneAttributes>[] endSharedData;
        private readonly KeyframeData<TKeyframeParameters, TBoneAttributes>[] endOnlyData;

        internal FrameTransition(
            TransitionJsonRepresentation json,
            Dictionary<string, Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>> keyframes,
            FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes> preceedingFrame
            )
        {
            if (json.Delay < 0)
                throw new InvalidDataException("Frame transition delay must be non-negative.");

            if (json.Time < 0)
                throw new InvalidDataException("Frame transition time must be non-negative.");

            this.Duration = json.Time;
            this.Delay = json.Delay;
            this.Transition = json.Transition;

            this.StartTime = preceedingFrame == null ? 0 : preceedingFrame.EndTime;
            this.StartFrame = preceedingFrame == null ? null : preceedingFrame.EndFrame;

            this.DelayEnd = this.StartTime + this.Delay;

            this.EndTime = this.StartTime + this.Delay + this.Duration;

            Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> endFrame;
            if(!keyframes.TryGetValue(json.Frame, out endFrame))
                throw new InvalidDataException("Frame transition must have valid key frame.");

            this.EndFrame = endFrame;

            if (this.StartFrame == null || this.StartFrame.Data.Count == 0)
            {
                var empty = new KeyframeData<TKeyframeParameters, TBoneAttributes>[0];
                this.startOnlyData = empty;
                this.startSharedData = empty;
                this.endSharedData = empty;
                this.endOnlyData = endFrame.Data.ToArray();
            }
            else if (this.EndFrame.Data.Count == 0)
            {
                var empty = new KeyframeData<TKeyframeParameters, TBoneAttributes>[0];
                this.startOnlyData = this.StartFrame.Data.ToArray();
                this.startSharedData = empty;
                this.endSharedData = empty;
                this.endOnlyData = empty;
            }
            else
            {
                var startonly = this.StartFrame.Data.ToList();
                var startshared = new List<KeyframeData<TKeyframeParameters, TBoneAttributes>>();
                var endshared = new List<KeyframeData<TKeyframeParameters, TBoneAttributes>>();
                var endonly = this.EndFrame.Data.ToList();

                for (int i = 0; i < startonly.Count; i++)
                {
                    if (endonly.Count == 0)
                        break;

                    var startbone = startonly[i].Bone;

                    for (int j = 0; j < endonly.Count; j++)
                    {
                        if (endonly[j].Bone == startbone)
                        {
                            startshared.Add(startonly[i]);
                            endshared.Add(endonly[j]);
                            startonly.RemoveAt(i);
                            endonly.RemoveAt(j);
                            i--;
                            break;
                        }
                    }
                }
                var empty = new KeyframeData<TKeyframeParameters, TBoneAttributes>[0];

                this.startOnlyData = startonly.Count == 0 ? empty : startonly.ToArray();
                this.startSharedData = startshared.Count == 0 ? empty : startshared.ToArray();
                this.endSharedData = endshared.Count == 0 ? empty : endshared.ToArray();
                this.endOnlyData = endonly.Count == 0 ? empty : endonly.ToArray();
            }
        }

        public void ApplyTo(TBoneParameters[] parameters, float t)
        {
            float s = 1 - t;

            foreach (var data in this.startOnlyData)
                parameters[data.Bone.Id].Add(data.Parameters, s);

            for (int i = 0; i < this.startSharedData.Length; i++)
            {
                var data1 = this.startSharedData[i];
                var data2 = this.endSharedData[i];
                parameters[data1.Bone.Id].Add(data1.Parameters, data2.Parameters, t);
            }

            foreach (var data in this.endOnlyData)
                parameters[data.Bone.Id].Add(data.Parameters, t);
        }
    }
}
