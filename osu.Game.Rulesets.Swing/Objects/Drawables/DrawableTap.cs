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
            AddRangeInternal(new Drawable[]
            {
                bar = new Box
                {
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2,
                    Width = 1,
                    EdgeSmoothness = Vector2.One,
                },
                contentContainer = new Container
                {
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = TapCircle = new DrawableTapCircle()
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

            bar.Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            bar.Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;

            bar.Colour = Type.Value == HitType.Up ? ColourInfo.GradientVertical(Color4.Black.Opacity(0), Color4.White) : ColourInfo.GradientVertical(Color4.White, Color4.Black.Opacity(0));

            contentContainer.Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            contentContainer.Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;

            TapCircle.Anchor = Type.Value == HitType.Up ? Anchor.BottomCentre : Anchor.TopCentre;
            TapCircle.Circle.Colour = tapCircleColour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        private bool hasNonMissResult => Result.HasResult && Result.Type != HitResult.Miss;

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current;

            if (currentTime < appearTime)
            {
                bar.Rotation = contentContainer.Rotation = Type.Value == HitType.Up ? -90 : 90;
            }
            else
            {
                var rotationOffset = (currentTime - appearTime) / rotationTime * 180;

                if (Type.Value == HitType.Up)
                {
                    if (!hasNonMissResult)
                        contentContainer.Rotation = (float)(-90 + rotationOffset);

                    bar.Rotation = (float)(-90 + rotationOffset);
                }
                else
                {
                    if (!hasNonMissResult)
                        contentContainer.Rotation = (float)(90 - rotationOffset);

                    bar.Rotation = (float)(90 - rotationOffset);
                }
            }
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
                    TapCircle.Circle.Colour = tapCircleColour;
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 100, Easing.OutQuint);
                    this.FadeOut(HitObject.TimePreempt / 3, Easing.Out);
                    break;

                case ArmedState.Hit:
                    TapCircle.ScaleTo(1.2f, 150, Easing.OutQuint);
                    TapCircle.Circle.FlashColour(Color4.White, 300, Easing.Out);
                    this.FadeOut(300, Easing.OutQuint);
                    break;
            }
        }
    }
}
