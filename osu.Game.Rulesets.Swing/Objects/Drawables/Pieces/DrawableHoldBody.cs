using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using System;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class DrawableHoldBody : CompositeDrawable
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();
        protected readonly Hold HitObject;

        private readonly PathSliderBody snakingBody;

        public DrawableHoldBody(Hold h)
        {
            HitObject = h;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            AddRangeInternal(new Drawable[]
            {
                snakingBody = new PathSliderBody()
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
            snakingBody.Colour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
        }

        public void StartTransform()
        {
            var foldDuration = Math.Min(HitObject.TimePreempt, HitObject.Duration);
            var maxFoldDegree = (float)Math.Min(HitObject.Duration, HitObject.TimePreempt) / HitObject.TimePreempt * 90;

            snakingBody.ProgressToDegree(maxFoldDegree, foldDuration);

            using (BeginDelayedSequence(Math.Max(HitObject.TimePreempt, HitObject.Duration), true))
                snakingBody.ProgressToDegree(0, foldDuration);
        }
    }
}
