﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Swing.UI;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldTick : DrawableSwingHitObject<HoldTick>
    {
        public bool Tracking { get; set; }

        private readonly Container contentContainer;
        private readonly TickElement tickElement;

        public DrawableHoldTick(HoldTick h)
            : base(h)
        {
            AddInternal(contentContainer = new Container
            {
                Height = SwingPlayfield.FULL_SIZE.Y / 2,
                Rotation = -90,
                Child = tickElement = new TickElement
                {
                    Anchor = Anchor.BottomCentre,
                }
            });
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
            contentContainer.RotateTo(0, HitObject.TimePreempt);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= 0)
                ApplyResult(r => r.Type = Tracking ? r.Judgement.MaxResult : HitResult.Miss);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            base.UpdateHitStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 50, Easing.Out);
                    this.FadeOut(200, Easing.Out);
                    break;

                case ArmedState.Hit:
                    tickElement.ScaleTo(1.3f, 200, Easing.Out);
                    this.FadeOut(200, Easing.Out);
                    break;
            }
        }

        public override bool OnPressed(SwingAction action) => false;
    }
}
