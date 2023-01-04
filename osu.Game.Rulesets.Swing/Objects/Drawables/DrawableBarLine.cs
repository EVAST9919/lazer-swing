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

        public DrawableBarLine(BarLine h)
            : base(h)
        {
            RelativeSizeAxes = Axes.Y;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Depth = -float.MaxValue;

            var size = h.Major ? 12 : 7;

            AddRangeInternal(new Drawable[]
            {
                topContainer = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 - 2,
                    Rotation = -90,
                    Child = new EquilateralTriangle
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Size = new Vector2(size),
                        EdgeSmoothness = Vector2.One
                    }
                },
                bottomContainer = new Container
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Height = SwingPlayfield.FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 - 2,
                    Rotation = 90,
                    Child = new EquilateralTriangle
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.BottomCentre,
                        Size = new Vector2(size),
                        Rotation = -180,
                        EdgeSmoothness = Vector2.One
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
