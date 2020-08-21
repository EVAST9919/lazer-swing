using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class HoldRepeat : SwingHitObject
    {
        public int RepeatIndex { get; set; }

        public override Judgement CreateJudgement() => new SwingJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
