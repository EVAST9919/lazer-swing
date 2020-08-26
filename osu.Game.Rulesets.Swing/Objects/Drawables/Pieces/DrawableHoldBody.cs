using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Extensions;
using osuTK.Graphics;
using System;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class DrawableHoldBody : CompositeDrawable
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();
        protected readonly Hold HitObject;

        private readonly PathSliderBody snakingBody;

        private readonly bool canFitOnTheScreen;
        private readonly double maxFoldDegree;
        private readonly double unfoldTime;
        private readonly double foldTime;

        public DrawableHoldBody(Hold h)
        {
            HitObject = h;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            InternalChild = snakingBody = new PathSliderBody();

            canFitOnTheScreen = h.Duration < h.TimePreempt;
            maxFoldDegree = (float)Math.Min(HitObject.Duration, HitObject.TimePreempt) / HitObject.TimePreempt * 90;

            unfoldTime = h.StartTime - h.TimePreempt;
            foldTime = unfoldTime + h.Duration;
        }

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current;

            if (currentTime < unfoldTime)
            {
                snakingBody.SetProgressDegree(0, 0);
                return;
            }

            if (canFitOnTheScreen)
            {
                if (currentTime < foldTime)
                {
                    snakingBody.SetProgressDegree(MathExtensions.Map(currentTime, unfoldTime, foldTime, 0, maxFoldDegree), 0);
                    return;
                }

                if (currentTime < HitObject.StartTime)
                {
                    var end = (float)MathExtensions.Map(currentTime, foldTime, HitObject.StartTime, 0, 90 - maxFoldDegree);
                    snakingBody.SetProgressDegree(end + maxFoldDegree, end);
                    return;
                }

                if (currentTime < HitObject.EndTime)
                {
                    snakingBody.SetProgressDegree(90, MathExtensions.Map(currentTime, HitObject.StartTime, HitObject.EndTime, 90 - maxFoldDegree, 90));
                    return;
                }
            }
            else
            {
                if (currentTime < HitObject.StartTime)
                {
                    snakingBody.SetProgressDegree(MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, 0, 90), 0);
                    return;
                }

                if (currentTime < foldTime)
                {
                    snakingBody.SetProgressDegree(90, 0);
                    return;
                }

                if (currentTime < HitObject.EndTime)
                {
                    snakingBody.SetProgressDegree(90, MathExtensions.Map(currentTime, foldTime, HitObject.EndTime, 0, 90));
                    return;
                }
            }

            snakingBody.SetProgressDegree(90, 90);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            snakingBody.Colour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        public void Kill()
        {
            snakingBody?.RecyclePath();
        }
    }
}
