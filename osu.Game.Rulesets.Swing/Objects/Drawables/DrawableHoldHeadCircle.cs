using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Swing.UI;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osuTK;
using osuTK.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldHeadCircle : DrawableSwingHitObject<HoldHeadCircle>
    {
        private readonly double rotationTime;
        private readonly double appearTime;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private readonly IHasPathWithRepeats path;
        private readonly Box bar;
        private readonly Container contentContainer;
        private readonly DrawableTapCircle tapCircle;

        public DrawableHoldHeadCircle(HoldHeadCircle h)
            : base(h)
        {
            path = h.Path;

            AddRangeInternal(new Drawable[]
            {
                bar = new Box
                {
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 + 1,
                    Width = 1,
                    EdgeSmoothness = Vector2.One,
                },
                contentContainer = new Container
                {
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = tapCircle = new DrawableTapCircle()
                }
            });

            tapCircle.Add(new Circle
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(20),
                Masking = true,
                EdgeEffect = new EdgeEffectParameters
                {
                    Colour = Color4.Black.Opacity(0.25f),
                    Type = EdgeEffectType.Shadow,
                    Radius = 10
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
                tapCircle.Circle.Colour = accent.NewValue;
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

            bar.Colour = Type.Value == HitType.Up ? ColourInfo.GradientVertical(Color4.Black.Opacity(0), Color4.White) : ColourInfo.GradientVertical(Color4.White, Color4.Black.Opacity(0));

            contentContainer.Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            contentContainer.Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;

            tapCircle.Anchor = Type.Value == HitType.Up ? Anchor.BottomCentre : Anchor.TopCentre;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
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
                bar.Rotation = Type.Value == HitType.Up ? (float)(-90 + rotationOffset) : (float)(90 - rotationOffset);

                if (currentTime < HitObject.StartTime)
                {
                    contentContainer.Rotation = Type.Value == HitType.Up ? (float)(-90 + rotationOffset) : (float)(90 - rotationOffset);
                }
                else if (currentTime < path.EndTime)
                {
                    contentContainer.Rotation = 0;
                    var offsetFromStart = currentTime - HitObject.StartTime;
                    if (offsetFromStart > HitObject.TimePreempt / 3)
                        bar.Alpha = 0;
                    else
                    {
                        bar.Alpha = 1 - (float)(offsetFromStart / (HitObject.TimePreempt / 3));
                    }
                }
                else
                {
                    if (hasNonMissResult)
                    {
                        contentContainer.Rotation = 0;
                    }
                    else
                    {
                        var postRotationOffset = (currentTime - appearTime - path.Duration) / rotationTime * 180;
                        contentContainer.Rotation = Type.Value == HitType.Up ? (float)(-90 + postRotationOffset) : (float)(90 - postRotationOffset);
                    }
                }
            }
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (timeOffset > path.Duration)
                    ApplyResult(r => r.Type = IsTracking ? HitResult.Perfect : HitResult.Miss);
            }
        }

        public bool IsTracking { get; private set; }

        public override bool OnPressed(SwingAction action)
        {
            if (Judged)
                return false;

            IsTracking = HitActions.Contains(action);
            return IsTracking;
        }

        public override void OnReleased(SwingAction action)
        {
            base.OnReleased(action);
            IsTracking = false;
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(path.Duration, true))
            {
                switch (state)
                {
                    case ArmedState.Miss:
                        this.FadeColour(Color4.Red, 100, Easing.OutQuint);
                        this.FadeOut(HitObject.TimePreempt / 3, Easing.Out);
                        break;

                    case ArmedState.Hit:
                        tapCircle.ScaleTo(1.2f, 150, Easing.OutQuint);
                        tapCircle.Circle.FlashColour(Color4.White, 300, Easing.Out);
                        this.FadeOut(HitObject.TimePreempt / 3, Easing.OutQuint);
                        break;
                }
            }
        }
    }
}
