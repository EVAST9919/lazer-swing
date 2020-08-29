using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableTap : DrawableSwingHitObject<Tap>
    {
        protected override bool RequiresTimedUpdate => true;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private bool validActionPressed;
        private readonly double rotationTime;
        private double appearTime;

        private readonly Box bar;
        private readonly Container contentContainer;
        public readonly DrawableTapCircle TapCircle;

        public DrawableTap(Tap h)
            : base(h)
        {
            Alpha = 0;
            AddRangeInternal(new Drawable[]
            {
                bar = new Box
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2,
                    Width = 1,
                    EdgeSmoothness = Vector2.One,
                    Colour = ColourInfo.GradientVertical(Color4.Black.Opacity(0), Color4.White)
                },
                contentContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = TapCircle = new DrawableTapCircle
                    {
                        Anchor = Anchor.BottomCentre
                    }
                }
            });

            rotationTime = h.TimePreempt * 2;
            h.StartTimeBindable.BindValueChanged(time =>
            {
                appearTime = time.NewValue - HitObject.TimePreempt;
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private Color4 tapCircleColour;

        private void updateType()
        {
            HitActions =
                HitObject.Type == HitType.Up
                    ? new[] { SwingAction.UpSwing, SwingAction.UpSwingAdditional }
                    : new[] { SwingAction.DownSwing, SwingAction.DownSwingAdditional };

            Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Scale = Type.Value == HitType.Up ? Vector2.One : new Vector2(1, -1);

            TapCircle.Colour = tapCircleColour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        private bool hasNonMissResult => Result.HasResult && Result.Type != HitResult.Miss;

        protected override void Update(double currentTime)
        {
            if (currentTime < appearTime)
            {
                bar.Rotation = contentContainer.Rotation = -90;
                return;
            }

            var rotationOffset = (float)(-90 + (currentTime - appearTime) / rotationTime * 180);

            if (!hasNonMissResult)
                contentContainer.Rotation = rotationOffset;

            bar.Rotation = rotationOffset;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
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
            var result = UpdateResult(true);
            if (IsHit)
                HitAction = action;

            return result;
        }

        public override void OnReleased(SwingAction action)
        {
            if (action == HitAction)
                HitAction = null;
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Idle:
                    this.FadeColour(Color4.White);
                    TapCircle.Circle.ClearTransforms();
                    TapCircle.Colour = tapCircleColour;
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 100, Easing.OutQuint);
                    this.FadeOut(HitObject.TimePreempt / 3, Easing.Out).Expire(true);
                    break;

                case ArmedState.Hit:
                    TapCircle.ScaleTo(1.2f, 150, Easing.OutQuint);
                    TapCircle.Circle.FlashColour(Color4.White, 300, Easing.Out);
                    this.FadeOut(300, Easing.OutQuint).Expire(true);
                    break;
            }
        }
    }
}
