using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Game.Rulesets.Swing.UI;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class HoldBall : CompositeDrawable
    {
        private readonly static float size = SwingHitObject.DEFAULT_SIZE / 2;

        public readonly BindableBool Tracking = new BindableBool();
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public readonly Ring Ring;

        public HoldBall()
        {
            Rotation = -90;
            AddInternal(new Container
            {
                Height = SwingPlayfield.FULL_SIZE.Y / 2,
                Child = Ring = new Ring(3.5f)
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(size)
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
                    Ring.ResizeTo(new Vector2(size * 1.5f), 75, Easing.Out);
                }
                else
                {
                    Ring.ResizeTo(new Vector2(size), 150, Easing.Out);
                }
            });
        }
    }
}
