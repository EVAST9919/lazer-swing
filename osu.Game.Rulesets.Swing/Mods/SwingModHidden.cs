using osu.Framework.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModHidden : ModHidden
    {
        public override string Description => @"Play with fading circles.";
        public override double ScoreMultiplier => 1.06;

        private const double fade_out_duration_multiplier = 0.3;

        protected override bool IsFirstHideableObject(DrawableHitObject hitObject) => !(hitObject is DrawableSpinner);

        protected override void ApplyHiddenState(DrawableHitObject drawable, ArmedState state)
        {
            if (!(drawable is DrawableSwingHitObject d))
                return;

            var h = d.HitObject;

            var fadeOutStartTime = h.StartTime - h.TimePreempt + h.TimePreempt / 3;
            var fadeOutDuration = h.TimePreempt * fade_out_duration_multiplier;

            switch (drawable)
            {
                case DrawableTap tap:
                    // fade out immediately after fade in.
                    using (tap.BeginAbsoluteSequence(fadeOutStartTime, true))
                        tap.FadeOut(fadeOutDuration);

                    break;

                case DrawableHold hold:
                    using (hold.BeginAbsoluteSequence(hold.HitObject.StartTime, true))
                        hold.FadeOut(hold.HitObject.Duration);
                    break;
            }
        }
    }
}
