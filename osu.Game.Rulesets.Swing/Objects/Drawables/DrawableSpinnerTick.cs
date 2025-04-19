using osu.Framework.Input.Events;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public partial class DrawableSpinnerTick : DrawableSwingHitObject<SpinnerTick>
    {
        public override bool DisplayResult => false;

        public DrawableSpinnerTick(SpinnerTick hitObject)
            : base(hitObject)
        {
        }

        public void TriggerResult(HitResult type)
        {
            HitObject.StartTime = Time.Current;
            ApplyResult((r, u) => r.Type = type);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
        }

        public override bool OnPressed(KeyBindingPressEvent<SwingAction> e) => false;
    }
}
