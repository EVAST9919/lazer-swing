using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class HoldHeadCircle : SwingHitObject
    {
        public IHasPathWithRepeats Path { get; set; }

        public override Judgement CreateJudgement() => new NullJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
