using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldBody : DrawableSwingHitObject<HoldBody>
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private readonly CircularProgress progress;

        private readonly bool canFitOnTheScreen;
        private readonly double finalFillValue;
        private readonly double unfoldTime;

        public DrawableHoldBody(HoldBody h)
            : base(h)
        {
            var thickness = SwingHitObject.DEFAULT_SIZE / 2;
            var size = SwingPlayfield.FULL_SIZE.X + thickness;

            AddRangeInternal(new Drawable[]
            {
                progress = new CircularProgress
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(size),
                    InnerRadius = thickness * 2 / size,
                    Current = { Value = 0 }
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
            progress.Colour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
        }

        public override bool OnPressed(SwingAction action) => false;

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current;

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

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset < HitObject.Duration)
                return;

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }
    }
}
