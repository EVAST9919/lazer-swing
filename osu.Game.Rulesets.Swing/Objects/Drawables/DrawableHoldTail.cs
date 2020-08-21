using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldTail : DrawableSwingHitObject<HoldTail>
    {
        public DrawableHoldTail(HoldTail h)
            : base(h)
        {

        }

        public override bool OnPressed(SwingAction action) => false;

        public void TriggerResult(HitResult result) => ApplyResult(r => r.Type = result);
    }
}
