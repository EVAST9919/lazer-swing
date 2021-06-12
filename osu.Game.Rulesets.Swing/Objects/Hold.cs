using osu.Framework.Bindables;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osuTK.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class Hold : SwingHitObject, IHasDuration, IHasDisplayColour
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

        public Hold()
        {
            TypeBindable.BindValueChanged(_ =>
            {
                DisplayColour.Value = Type == HitType.Up ? Tap.COLOUR_TOP : Tap.COLOUR_BOTTOM;
            });
        }

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
        public HoldTail Tail;

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
                }
            }

            AddNested(Tail = new HoldTail
            {
                StartTime = EndTime,
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
                sampleList.Add(new HitSampleInfo("slidertick", firstSample.Bank, null, firstSample.Volume));
            }

            if (HeadCircle != null)
                HeadCircle.Samples = getNodeSamples(0);

            if (Tail != null)
                Tail.Samples = Samples;
        }

        private IList<HitSampleInfo> getNodeSamples(int nodeIndex) =>
            nodeIndex < NodeSamples.Count ? NodeSamples[nodeIndex] : Samples;

        public Bindable<Color4> DisplayColour { get; } = new Bindable<Color4>(Tap.COLOUR_TOP);
    }
}
