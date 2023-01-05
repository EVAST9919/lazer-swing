using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Game.Rulesets.Swing.UI;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public partial class HoldBall : CompositeDrawable
    {
        private static readonly float size = SwingHitObject.DEFAULT_SIZE / 1.5f;

        public readonly BindableBool Tracking = new BindableBool();
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public readonly Ring Ring;

        public HoldBall()
        {
            Rotation = -90;
            AddInternal(new Container
            {
                Height = SwingPlayfield.FULL_SIZE.Y / 2,
                Child = Ring = new Ring(4)
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
                    Ring.ResizeTo(new Vector2(size * 1.3f), 75, Easing.Out);
                }
                else
                {
                    Ring.ResizeTo(new Vector2(size), 150, Easing.Out);
                }
            });
        }
    }
}
