using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Judgements;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class HoldTick : SwingHitObject
    {
        public bool UsesRepeatSound { get; set; }

        public int RepeatIndex { get; set; }

        public override Judgement CreateJudgement() => new SwingJudgement();

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
