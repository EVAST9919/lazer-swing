using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class HoldTick : SwingHitObject
    {
        public override Judgement CreateJudgement() => new HoldTickJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;

        public class HoldTickJudgement : SwingJudgement
        {
            protected override int NumericResultFor(HitResult result) => result == MaxResult ? 10 : 0;
        }
    }
}
