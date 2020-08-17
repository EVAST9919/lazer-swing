using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Judgements
{
    public class SwingJudgement : Judgement
    {
        public override HitResult MaxResult => HitResult.Great;

        protected override int NumericResultFor(HitResult result)
        {
            switch (result)
            {
                case HitResult.Good:
                    return 100;

                case HitResult.Great:
                    return 300;

                default:
                    return 0;
            }
        }
    }
}
