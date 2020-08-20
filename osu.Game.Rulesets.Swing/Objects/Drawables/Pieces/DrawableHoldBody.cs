﻿using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
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

        public DrawableHoldBody(Hold h)
        {
            HitObject = h;

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
                        X = 0.3f, // temporary
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
                        Anchor = Anchor.BottomCentre,
                        X = -0.1f
                    }
                }
            });

            canFitOnTheScreen = h.Duration < h.TimePreempt;
            if (canFitOnTheScreen)
                finalFillValue = MathExtensions.Map(h.Duration, 0, h.TimePreempt, 0, 90);

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
                if (currentTime < unfoldTime + HitObject.Duration)
                {
                    snakingBody.Rotation = 0;
                    snakingBody.UnfoldDegree = MathExtensions.Map(currentTime, unfoldTime, unfoldTime + HitObject.Duration, 0, finalFillValue);
                    return;
                }

                if (currentTime < HitObject.StartTime)
                {
                    snakingBody.UnfoldDegree = finalFillValue;
                    snakingBody.Rotation = (float)MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.StartTime, 0, 90 - finalFillValue);
                    return;
                }

                if (currentTime < HitObject.StartTime + HitObject.Duration)
                {
                    snakingBody.UnfoldDegree = finalFillValue - MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.StartTime + HitObject.Duration, 0, finalFillValue);
                    snakingBody.Rotation = (float)MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.StartTime + HitObject.Duration, (90 - finalFillValue), 90);
                    return;
                }

                snakingBody.Rotation = 90;
                snakingBody.UnfoldDegree = 0;
            }
            else
            {
                if (currentTime < HitObject.StartTime)
                {
                    snakingBody.Rotation = 0;
                    snakingBody.UnfoldDegree = MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, 0, 90);
                    return;
                }

                if (currentTime < unfoldTime + HitObject.Duration)
                {
                    snakingBody.Rotation = 0;
                    snakingBody.UnfoldDegree = 90;
                    return;
                }

                if (currentTime < HitObject.EndTime)
                {
                    snakingBody.Rotation = (float)MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.EndTime, 0, 90);
                    snakingBody.UnfoldDegree = 90 - MathExtensions.Map(currentTime, unfoldTime + HitObject.Duration, HitObject.EndTime, 0, 90);
                    return;
                }

                snakingBody.Rotation = 90;
                snakingBody.UnfoldDegree = 0;
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
    }
}
