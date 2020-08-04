﻿using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableSpinnerTick : DrawableSwingHitObject<SpinnerTick>
    {
        public override bool DisplayResult => false;

        public DrawableSpinnerTick(SpinnerTick hitObject)
            : base(hitObject)
        {
        }

        public void TriggerResult(HitResult type)
        {
            HitObject.StartTime = Time.Current;
            ApplyResult(r => r.Type = type);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
        }

        public override bool OnPressed(SwingAction action) => false;
    }
}
