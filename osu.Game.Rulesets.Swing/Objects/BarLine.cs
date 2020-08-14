using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class BarLine : SwingHitObject
    {
        public bool Major { get; set; }

        public override Judgement CreateJudgement() => new NullJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
