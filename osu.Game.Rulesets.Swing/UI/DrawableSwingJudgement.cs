using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Swing.UI
{
    public class DrawableSwingJudgement : DrawableJudgement
    {
        public DrawableSwingJudgement(JudgementResult result, DrawableHitObject judgedObject)
            : base(result, judgedObject)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            switch (Result.Type)
            {
                case HitResult.Good:
                    JudgementBody.Colour = colours.GreenLight;
                    break;

                case HitResult.Great:
                    JudgementBody.Colour = colours.BlueLight;
                    break;
            }
        }
    }
}
