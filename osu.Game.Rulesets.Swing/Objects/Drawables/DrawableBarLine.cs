using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Swing.UI;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public partial class DrawableBarLine : DrawableSwingHitObject<BarLine>
    {
        private readonly double rotationDuration;
        private readonly double fadeDuration;

        private readonly Container topContainer;
        private readonly Container bottomContainer;

        private static Vector2 major_size = new Vector2(3, 10);
        private static Vector2 minor_size = new Vector2(2, 7);

        public DrawableBarLine(BarLine h)
            : base(h)
        {
            RelativeSizeAxes = Axes.Y;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Depth = -float.MaxValue;

            var size = h.Major ? major_size : minor_size;

            AddRangeInternal(new Drawable[]
            {
                topContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 - 2,
                    Rotation = -90,
                    Child = new Circle
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Size = size
                    }
                },
                bottomContainer = new Container
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 - 2,
                    Rotation = 90,
                    Child = new Circle
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.BottomCentre,
                        Size = size,
                        Rotation = -180
                    }
                }
            });

            fadeDuration = h.TimePreempt;
            rotationDuration = h.TimePreempt * 2;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            this.FadeInFromZero(fadeDuration);
            topContainer.RotateTo(90, rotationDuration);
            bottomContainer.RotateTo(-90, rotationDuration);
            this.Delay(rotationDuration - fadeDuration).FadeOut(fadeDuration).Expire(true);
        }

        public override bool OnPressed(KeyBindingPressEvent<SwingAction> e) => false;
    }
}
