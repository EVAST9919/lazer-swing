using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldTail : DrawableSwingHitObject<HoldTail>
    {
        public bool Tracking { get; set; }

        public DrawableHoldTail(HoldTail h)
            : base(h)
        {

        }

        public override bool OnPressed(SwingAction action) => false;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= 0)
                ApplyResult(r => r.Type = Tracking ? r.Judgement.MaxResult : HitResult.Miss);
        }
    }
}
