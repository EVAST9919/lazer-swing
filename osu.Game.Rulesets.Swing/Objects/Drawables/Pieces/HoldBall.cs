using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osuTK.Graphics;
using osu.Game.Rulesets.Swing.UI;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class HoldBall : CompositeDrawable
    {
        public readonly BindableBool Tracking = new BindableBool();
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private readonly DrawableTapCircle circle;

        public HoldBall()
        {
            Rotation = -90;
            AddInternal(new Container
            {
                Height = SwingPlayfield.FULL_SIZE.Y / 2,

                Child = circle = new DrawableTapCircle
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(0.5f),
                    Colour = Color4.Transparent
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
