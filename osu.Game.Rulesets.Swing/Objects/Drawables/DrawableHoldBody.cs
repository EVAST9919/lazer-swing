using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osu.Game.Screens.Ranking.Expanded.Accuracy;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldBody : DrawableSwingHitObject<HoldBody>
    {
        protected override bool RequiresTimedUpdate => true;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private readonly SmoothCircularProgress progress;
        private readonly Container headContainer;
        private readonly Container tailContainer;
        private readonly Circle head;
        private readonly Circle tail;

        private readonly bool canFitOnTheScreen;
        private readonly double finalFillValue;
        private readonly double unfoldTime;

        public DrawableHoldBody(HoldBody h)
            : base(h)
        {
            var thickness = SwingHitObject.DEFAULT_SIZE / 2;
            var size = SwingPlayfield.FULL_SIZE.X + thickness;
            var innerRadius = thickness * 2 / size;

            AddRangeInternal(new Drawable[]
            {
                progress = new SmoothCircularProgress
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(size),
                    InnerRadius = innerRadius,
                    Current = { Value = 0 },
                },
                headContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = head = new Circle
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(thickness - 1)
                    }
                },
                tailContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = tail = new Circle
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(thickness - 1)
                    }
                }
            });

            canFitOnTheScreen = h.Duration < h.TimePreempt;
            if (canFitOnTheScreen)
                finalFillValue = MathExtensions.Map(h.Duration, 0, h.TimePreempt, 0, 0.25f);

            unfoldTime = h.StartTime - h.TimePreempt;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Scale = Type.Value == HitType.Up ? Vector2.One : new Vector2(1, -1);
            progress.Colour = head.Colour = tail.Colour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
        }

        public override bool OnPressed(SwingAction action) => false;

        protected override void Update(double currentTime)
        {
            updateHeadRotation(currentTime);
            updateTailRotation(currentTime);

            if (currentTime < unfoldTime)
            {
                progress.Current.Value = 0;
                progress.Rotation = 90;
                return;
            }

            if (canFitOnTheScreen)
            {
                if (currentTime < unfoldTime + HitObject.Duration)
                {
                    progress.Rotation = 90;
                    progress.Current.Value = MathExtensions.Map(currentTime, unfoldTime, unfoldTime + HitObject.Duration, 0, finalFillValue);
                    return;
                }

                if (currentTime < HitObject.StartTime)
                {
                    progress.Current.Value = finalFillValue;
                    progress.Rotation = 90 + (float)MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.StartTime, 0, (0.25 - finalFillValue) * 360);
                    return;
                }

                if (currentTime < HitObject.StartTime + HitObject.Duration)
                {
                    progress.Rotation = 90 + (float)MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.StartTime + HitObject.Duration, (0.25 - finalFillValue) * 360, 90);
                    progress.Current.Value = finalFillValue - MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.StartTime + HitObject.Duration, 0, finalFillValue);
                    return;
                }

                progress.Rotation = 180;
                progress.Current.Value = 0;
            }
            else
            {
                if (currentTime < HitObject.StartTime)
                {
                    progress.Rotation = 90;
                    progress.Current.Value = MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, 0, 0.25);
                    return;
                }

                if (currentTime < unfoldTime + HitObject.Duration)
                {
                    progress.Rotation = 90;
                    progress.Current.Value = 0.25;
                    return;
                }

                if (currentTime < HitObject.EndTime)
                {
                    progress.Rotation = (float)MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.EndTime, 90, 180);
                    progress.Current.Value = 0.25 - MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.EndTime, 0, 0.25);
                    return;
                }

                progress.Rotation = 180;
                progress.Current.Value = 0;
            }
        }

        private void updateHeadRotation(double currentTime)
        {
            if (currentTime < unfoldTime)
            {
                headContainer.Rotation = -90;
                return;
            }

            if (currentTime < HitObject.StartTime)
            {
                headContainer.Rotation = (float)MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, -90, 0);
                return;
            }

            headContainer.Rotation = 0;
        }

        private void updateTailRotation(double currentTime)
        {
            if (currentTime < unfoldTime + HitObject.Duration)
            {
                tailContainer.Rotation = -90;
                return;
            }

            if (canFitOnTheScreen)
            {
                if (currentTime < HitObject.StartTime + HitObject.Duration)
                {
                    tailContainer.Rotation = (float)MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.StartTime + HitObject.Duration, -90, 0);
                    return;
                }
            }
            else
            {
                if (currentTime < HitObject.EndTime)
                {
                    tailContainer.Rotation = (float)MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.EndTime, -90, 0);
                    return;
                }
            }

            tailContainer.Rotation = 0;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset < HitObject.Duration)
                return;

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);
            this.Delay(HitObject.Duration).FadeOut(300, Easing.OutQuint).Expire(true);
        }
    }
}
