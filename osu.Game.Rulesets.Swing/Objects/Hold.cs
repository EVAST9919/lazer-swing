using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        public IHasPathWithRepeats Path { get; set; }

        public double SpanDuration => Path.Duration / Path.SpanCount();

        public double Velocity { get; private set; }

        public double TickDistance { get; private set; }

        public List<IList<HitSampleInfo>> NodeSamples { get; set; } = new List<IList<HitSampleInfo>>();

        public override Judgement CreateJudgement() => new IgnoreJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = 100 * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            Velocity = scoringDistance / timingPoint.BeatLength;
            TickDistance = scoringDistance / difficulty.SliderTickRate;
        }

        public HoldHead HeadCircle;

        protected override void CreateNestedHitObjects(CancellationToken cancellationToken)
        {
            base.CreateNestedHitObjects(cancellationToken);

            foreach (var e in SliderEventGenerator.Generate(StartTime, SpanDuration, Velocity, TickDistance, Path.Distance, Path.SpanCount(), null, cancellationToken))
            {
                switch (e.Type)
                {
                    case SliderEventType.Head:
                        AddNested(HeadCircle = new HoldHead
                        {
                            StartTime = e.Time,
                            Type = Type
                        });
                        break;

                    case SliderEventType.Tick:
                        AddNested(new HoldTick
                        {
                            StartTime = e.Time,
                            Type = Type
                        });
                        break;

                    case SliderEventType.Repeat:
                        AddNested(new HoldTick
                        {
                            RepeatIndex = e.SpanIndex,
                            StartTime = e.Time,
                            Type = Type
                        });
                        break;
                }
            }

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

            foreach (var tick in NestedHitObjects.OfType<HoldTick>())
                tick.Samples = tick.UsesRepeatSound ? getNodeSamples(tick.RepeatIndex + 1) : sampleList;
        }

        private IList<HitSampleInfo> getNodeSamples(int nodeIndex) =>
            nodeIndex < NodeSamples.Count ? NodeSamples[nodeIndex] : Samples;
    }
}
