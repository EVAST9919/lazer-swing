using System.Threading;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;
using osu.Framework.Bindables;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class Spinner : SwingHitObject, IHasDuration, IHasDisplayColour
    {
        public Bindable<Color4> DisplayColour => new Bindable<Color4>(Color4.BlueViolet);

        public double EndTime
        {
            get => StartTime + Duration;
            set => Duration = value - StartTime;
        }

        public double Duration { get; set; }

        /// <summary>
        /// The number of hits required to complete the spinner successfully.
        /// </summary>
        public int RequiredHits = 10;

        protected override void CreateNestedHitObjects(CancellationToken cancellationToken)
        {
            base.CreateNestedHitObjects(cancellationToken);

            for (int i = 0; i < RequiredHits; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                AddNested(new SpinnerTick());
            }
        }

        public override Judgement CreateJudgement() => new SwingJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
