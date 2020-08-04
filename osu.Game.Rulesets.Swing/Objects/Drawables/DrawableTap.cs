using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.UI;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osuTK;
using osuTK.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableTap : DrawableSwingHitObject<Tap>
    {
        private bool validActionPressed;
        private readonly double rotationTime;
        private readonly double appearTime;

        private readonly Box circle;
        private readonly Container content;
        private readonly Box bar;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public DrawableTap(Tap h)
            : base(h)
        {
            AutoSizeAxes = Axes.Both;
            AddInternal(new Container
            {
                AutoSizeAxes = Axes.X,
                Height = SwingPlayfield.FULL_SIZE.Y / 2,
                Children = new Drawable[]
                {
                    bar = new Box
                    {
                        Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 + 1,
                        Width = 1,
                        EdgeSmoothness = Vector2.One,
                    },
                    content = new Container
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(SwingHitObject.DEFAULT_SIZE),
                        Child = new CircularContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Masking = true,
                            BorderThickness = 4,
                            BorderColour = Color4.White,
                            Child = circle = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                            EdgeEffect = new EdgeEffectParameters
                            {
                                Colour = Color4.Black.Opacity(0.25f),
                                Type = EdgeEffectType.Shadow,
                                Radius = 10
                            }
                        }
                    }
                }
            });

            rotationTime = h.TimePreempt * 2;
            appearTime = HitObject.StartTime - HitObject.TimePreempt;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);

            AccentColour.BindValueChanged(accent =>
            {
                circle.Colour = accent.NewValue;
            }, true);
        }

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
            content.Anchor = Type.Value == HitType.Up ? Anchor.BottomCentre : Anchor.TopCentre;
        }

        protected override void Update()
        {
            base.Update();

            if (Result.HasResult && Result.Type != HitResult.Miss)
                return;

            var currentTime = Time.Current;

            if (currentTime < appearTime)
                Rotation = Type.Value == HitType.Up ? -90 : 90;
            else
            {
                var rotationOffset = (currentTime - appearTime) / rotationTime * 180;
                if (Type.Value == HitType.Up)
                {
                    Rotation = (float)(-90 + rotationOffset);
                }
                else
                {
                    Rotation = (float)(90 - rotationOffset);
                }
            }
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 5, Easing.Out);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
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
                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 100, Easing.OutQuint);
                    this.FadeOut(100);
                    break;

                case ArmedState.Hit:
                    content.ScaleTo(1.2f, 150, Easing.OutQuint);
                    circle.FlashColour(Color4.White, 300, Easing.Out);
                    bar.FadeOut();
                    this.FadeOut(300, Easing.OutQuint);
                    break;
            }
        }
    }
}
