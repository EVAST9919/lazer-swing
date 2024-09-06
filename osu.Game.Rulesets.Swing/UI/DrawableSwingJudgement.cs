using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.UI
{
    public partial class DrawableSwingJudgement : DrawableJudgement
    {
        [Resolved]
        private OsuColour colours { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public override void Apply(JudgementResult result, DrawableHitObject judgedObject)
        {
            base.Apply(result, judgedObject);

            if (JudgementBody == null)
                return;

            switch (result.Type)
            {
                case HitResult.Good:
                    JudgementBody.Colour = colours.GreenLight;
                    break;

                case HitResult.Great:
                    JudgementBody.Colour = colours.BlueLight;
                    break;
            }
        }

        protected override Drawable CreateDefaultJudgement(HitResult result) => new DefaultSwingJudgementPiece(result);

        private partial class DefaultSwingJudgementPiece : DefaultJudgementPiece
        {
            public DefaultSwingJudgementPiece(HitResult result)
                : base(result)
            {
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                JudgementText.Font = JudgementText.Font.With(size: 25);
            }

            public override void PlayAnimation()
            {
                switch (Result)
                {
                    case HitResult.None:
                    case HitResult.Miss:
                        base.PlayAnimation();
                        break;

                    default:
                        this.ScaleTo(0.8f);
                        this.ScaleTo(1, 250, Easing.OutElastic);

                        this.Delay(50)
                            .ScaleTo(0.75f, 250)
                            .FadeOut(200);
                        break;
                }
            }
        }
    }
}
