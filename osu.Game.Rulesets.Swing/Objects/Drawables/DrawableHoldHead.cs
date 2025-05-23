﻿using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Scoring;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public partial class DrawableHoldHead : DrawableSwingHitObject<HoldHead>
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
                ApplyResult((r, u) => r.Type = r.Judgement.MaxResult);
                return;
            }

            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                {
                    ApplyResult((r, u) => r.Type = HitResult.Miss);
                }

                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            if (validActionPressed)
                ApplyResult((r, u) => r.Type = result);
        }

        public override bool OnPressed(KeyBindingPressEvent<SwingAction> e)
        {
            if (Judged)
                return false;

            validActionPressed = HitActions.Contains(e.Action);

            // Only count this as handled if the new judgement is a hit
            UpdateResult(true);

            if (IsHit)
                HitAction = e.Action;

            return false;
        }

        public override void OnReleased(KeyBindingReleaseEvent<SwingAction> e)
        {
            if (e.Action == HitAction)
                HitAction = null;
        }
    }
}
