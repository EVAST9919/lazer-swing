using osu.Game.Audio;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class Hold : SwingHitObject, IHasDuration
    {
        public double EndTime
        {
            get => StartTime + Duration;
            set => Duration = value - StartTime;
        }

        public double Duration { get; set; }

        public List<IList<HitSampleInfo>> NodeSamples { get; set; } = new List<IList<HitSampleInfo>>();

        public HoldHead HeadCircle;

        public override Judgement CreateJudgement() => new IgnoreJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            AddNested(new HoldBody
            {
                StartTime = StartTime,
                Duration = Duration,
                Type = Type
            });

            AddNested(HeadCircle = new HoldHead
            {
                StartTime = StartTime,
                Type = Type
            });

            updateNestedSamples();
        }

        private void updateNestedSamples()
        {
            var firstSample = Samples.FirstOrDefault(s => s.Name == HitSampleInfo.HIT_NORMAL)
                              ?? Samples.FirstOrDefault(); // TODO: remove this when guaranteed sort is present for samples (https://github.com/ppy/osu/issues/1933)
            var sampleList = new List<HitSampleInfo>();

            if (firstSample != null)
            {
                sampleList.Add(new HitSampleInfo
                {
                    Bank = firstSample.Bank,
                    Volume = firstSample.Volume,
                    Name = @"slidertick",
                });
            }

            if (HeadCircle != null)
                HeadCircle.Samples = getNodeSamples(0);
        }

        private IList<HitSampleInfo> getNodeSamples(int nodeIndex) =>
            nodeIndex < NodeSamples.Count ? NodeSamples[nodeIndex] : Samples;
    }
}
