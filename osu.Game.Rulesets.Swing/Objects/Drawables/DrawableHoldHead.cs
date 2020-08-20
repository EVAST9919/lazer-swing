using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Scoring;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldHead : DrawableSwingHitObject<HoldHead>
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private bool validActionPressed;

        public DrawableHoldHead(HoldHead h)
            : base(h)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            HitActions =
                HitObject.Type == HitType.Up
                    ? new[] { SwingAction.UpSwing, SwingAction.UpSwingAdditional }
                    : new[] { SwingAction.DownSwing, SwingAction.DownSwingAdditional };
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (Auto && timeOffset >= 0)
            {
                ApplyResult(r => r.Type = r.Judgement.MaxResult);
                return;
            }

            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                {
                    ApplyResult(r => r.Type = HitResult.Miss);
                }
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            if (validActionPressed)
                ApplyResult(r => r.Type = result);
        }

        public override bool OnPressed(SwingAction action)
        {
            if (Judged)
                return false;

            validActionPressed = HitActions.Contains(action);

            // Only count this as handled if the new judgement is a hit
            UpdateResult(true);

            if (IsHit)
                HitAction = action;

            return false;
        }

        public override void OnReleased(SwingAction action)
        {
            if (action == HitAction)
                HitAction = null;
        }
    }
}
