using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class DrawableHoldBody : CompositeDrawable
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();
        protected readonly Hold HitObject;

        private readonly SnakingHoldBody snakingBody;
        private readonly Container headContainer;
        private readonly Container tailContainer;
        private readonly HoldBodyEnd head;
        private readonly HoldBodyEnd tail;

        private readonly bool canFitOnTheScreen;
        private readonly double finalFillValue;
        private readonly double unfoldTime;
        private readonly double foldTime;

        public DrawableHoldBody(Hold h)
        {
            HitObject = h;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            AddRangeInternal(new Drawable[]
            {
                snakingBody = new SnakingHoldBody(),
                headContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = head = new HoldBodyEnd
                    {
                        Anchor = Anchor.BottomCentre,
                        Rotation = 180
                    }
                },
                tailContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = tail = new HoldBodyEnd
                    {
                        Anchor = Anchor.BottomCentre
                    }
                }
            });

            canFitOnTheScreen = h.Duration < h.TimePreempt;
            if (canFitOnTheScreen)
                finalFillValue = MathExtensions.Map(h.Duration, 0, h.TimePreempt, 0, 90);

            unfoldTime = h.StartTime - h.TimePreempt;
            foldTime = unfoldTime + h.Duration;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            snakingBody.Colour = head.Colour = tail.Colour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current;

            updateHeadRotation(currentTime);
            updateTailRotation(currentTime);

            if (currentTime < unfoldTime)
            {
                snakingBody.UnfoldDegree = 0;
                snakingBody.Rotation = 0;
                return;
            }

            if (canFitOnTheScreen)
            {
                if (currentTime < foldTime)
                {
                    snakingBody.Rotation = 0;
                    snakingBody.UnfoldDegree = MathExtensions.Map(currentTime, unfoldTime, foldTime, 0, finalFillValue);
                    return;
                }

                if (currentTime < HitObject.StartTime)
                {
                    snakingBody.UnfoldDegree = finalFillValue;
                    snakingBody.Rotation = (float)MathExtensions.Map(currentTime, foldTime, HitObject.StartTime, 0, 90 - finalFillValue);
                    return;
                }

                if (currentTime < HitObject.EndTime)
                {
                    snakingBody.UnfoldDegree = finalFillValue - MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.EndTime, 0, finalFillValue);
                    snakingBody.Rotation = 90 - (float)MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.EndTime, finalFillValue, 0);
                    return;
                }
            }
            else
            {
                if (currentTime < HitObject.StartTime)
                {
                    snakingBody.Rotation = 0;
                    snakingBody.UnfoldDegree = MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, 0, 90);
                    return;
                }

                if (currentTime < foldTime)
                {
                    snakingBody.Rotation = 0;
                    snakingBody.UnfoldDegree = 90;
                    return;
                }

                if (currentTime < HitObject.EndTime)
                {
                    snakingBody.Rotation = (float)MathExtensions.Map(currentTime, foldTime, HitObject.EndTime, 0, 90);
                    snakingBody.UnfoldDegree = 90 - MathExtensions.Map(currentTime, foldTime, HitObject.EndTime, 0, 90);
                    return;
                }
            }

            snakingBody.Rotation = 90;
            snakingBody.UnfoldDegree = 0;
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
            if (currentTime < foldTime)
            {
                tailContainer.Rotation = -90;
                return;
            }

            if (currentTime < HitObject.EndTime)
            {
                tailContainer.Rotation = (float)MathExtensions.Map(currentTime, foldTime, HitObject.EndTime, -90, 0);
                return;
            }

            tailContainer.Rotation = 0;
        }
    }
}
