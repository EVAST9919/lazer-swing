using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Extensions;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class DrawableHoldBody : CompositeDrawable
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();
        protected readonly Hold HitObject;

        private readonly PathSliderBody snakingBody;

        private readonly double unfoldTime;
        private readonly double foldTime;

        public DrawableHoldBody(Hold h)
        {
            HitObject = h;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            InternalChild = snakingBody = new PathSliderBody();

            unfoldTime = h.StartTime - h.TimePreempt;
            foldTime = unfoldTime + h.Duration;
        }

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current;

            var tailValue = MathExtensions.Map(currentTime, foldTime, HitObject.EndTime, 0, 90);
            var headValue = MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, 0, 90);

            snakingBody.SetProgressDegree(headValue, tailValue);
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
