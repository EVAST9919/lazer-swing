using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class Hold : SwingHitObject
    {
        public IHasPathWithRepeats Path { get; set; }

        public double Velocity { get; private set; }

        public double TickDistance { get; private set; }

        public double TickDistanceMultiplier = 1;

        public double SpanDuration => Path.Duration / Path.SpanCount();

        public double? LegacyLastTickOffset { get; set; }

        public List<IList<HitSampleInfo>> NodeSamples { get; set; } = new List<IList<HitSampleInfo>>();

        public override Judgement CreateJudgement() => new NullJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;

        public HoldHeadCircle HeadCircle;
        public HoldHeadSound HeadSound;
        public HoldTailCircle TailCircle;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = 100 * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            Velocity = scoringDistance / timingPoint.BeatLength;
            TickDistance = scoringDistance / difficulty.SliderTickRate * TickDistanceMultiplier;
        }

        protected override void CreateNestedHitObjects(CancellationToken cancellationToken)
        {
            base.CreateNestedHitObjects(cancellationToken);

            foreach (var e in SliderEventGenerator.Generate(StartTime, SpanDuration, Velocity, TickDistance, Path.Distance, Path.SpanCount(), LegacyLastTickOffset, cancellationToken))
            {
                switch (e.Type)
                {
                    case SliderEventType.Tick:
                        //AddNested(new HoldTick
                        //{
                        //    SpanIndex = e.SpanIndex,
                        //    SpanStartTime = e.SpanStartTime,
                        //    StartTime = e.Time,
                        //});
                        break;

                    case SliderEventType.Head:
                        AddNested(HeadSound = new HoldHeadSound
                        {
                            Type = Type,
                            StartTime = e.Time
                        });
                        AddNested(HeadCircle = new HoldHeadCircle
                        {
                            Type = Type,
                            Path = Path,
                            StartTime = e.Time,
                            SampleControlPoint = SampleControlPoint,
                        });
                        break;

                    case SliderEventType.LegacyLastTick:
                        break;

                    case SliderEventType.Repeat:
                        //AddNested(new HoldRepeat
                        //{
                        //    RepeatIndex = e.SpanIndex,
                        //    SpanDuration = SpanDuration,
                        //    StartTime = StartTime + (e.SpanIndex + 1) * SpanDuration,
                        //});
                        break;
                }
            }

            AddNested(TailCircle = new HoldTailCircle
            {
                Type = Type,
                StartTime = Path.EndTime
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

            //foreach (var tick in NestedHitObjects.OfType<HoldTick>())
            //    tick.Samples = sampleList;

            //foreach (var repeat in NestedHitObjects.OfType<HoldRepeat>())
            //    repeat.Samples = getNodeSamples(repeat.RepeatIndex + 1);

            if (TailCircle != null)
                TailCircle.Samples = getNodeSamples(0);

            if (HeadSound != null)
                HeadSound.Samples = getNodeSamples(0);
        }

        private IList<HitSampleInfo> getNodeSamples(int nodeIndex) =>
            nodeIndex < NodeSamples.Count ? NodeSamples[nodeIndex] : Samples;
    }
}
