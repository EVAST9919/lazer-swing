using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Game.Rulesets.Swing.UI;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class HoldBall : CompositeDrawable
    {
        public readonly BindableBool Tracking = new BindableBool();
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private readonly TapCircleOverlay circle;

        public HoldBall()
        {
            Rotation = -90;
            AddInternal(new Container
            {
                Height = SwingPlayfield.FULL_SIZE.Y / 2,

                Child = circle = new TapCircleOverlay
                {
                    Anchor = Anchor.BottomCentre,
                    Scale = new Vector2(0.5f)
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Tracking.BindValueChanged(tracking =>
            {
                if (tracking.NewValue)
                {
                    circle.ScaleTo(0.75f, 75, Easing.Out);
                }
                else
                {
                    circle.ScaleTo(0.5f, 150, Easing.Out);
                }
            }, true);
        }

        public void EndAnimation(double duration)
        {
            circle.ScaleTo(1, duration, Easing.Out);
        }
    }
}
