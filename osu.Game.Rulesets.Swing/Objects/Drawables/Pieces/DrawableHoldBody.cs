using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osuTK.Graphics;
using System;

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

        public DrawableHoldBody(Hold h)
        {
            HitObject = h;

            AutoSizeAxes = Axes.Both;
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
                    Rotation = -90,
                    Child = head = new HoldBodyEnd
                    {
                        Anchor = Anchor.BottomCentre,
                        Rotation = 180,
                        X = 0.5f
                    }
                },
                tailContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Rotation = -90,
                    Child = tail = new HoldBodyEnd
                    {
                        Anchor = Anchor.BottomCentre
                    }
                }
            });
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

        public void StartTransform()
        {
            headContainer.RotateTo(0, HitObject.TimePreempt);
            tailContainer.Delay(HitObject.Duration).RotateTo(0, HitObject.TimePreempt);

            var foldDuration = Math.Min(HitObject.TimePreempt, HitObject.Duration);
            var canFitOnScreen = HitObject.Duration < HitObject.TimePreempt;
            var maxFoldDegree = canFitOnScreen ? (float)MathExtensions.Map(HitObject.Duration, 0, HitObject.TimePreempt, 0, 90) : 90;

            snakingBody.Delay(HitObject.Duration).RotateTo(90, HitObject.TimePreempt);
            snakingBody.UnfoldToDegree(maxFoldDegree, foldDuration);

            using (BeginDelayedSequence(Math.Max(HitObject.TimePreempt, HitObject.Duration), true))
                snakingBody.UnfoldToDegree(0, foldDuration);
        }
    }
}
