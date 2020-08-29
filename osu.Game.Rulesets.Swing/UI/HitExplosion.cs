using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osu.Game.Rulesets.Swing.Objects;
using osu.Framework.Graphics.Effects;

namespace osu.Game.Rulesets.Swing.UI
{
    public class HitExplosion : CompositeDrawable
    {
        private readonly ExplosiveLine topLine;
        private readonly ExplosiveLine bottomLine;

        public HitExplosion(DrawableSwingHitObject h)
        {
            Color4 colour;

            switch (h)
            {
                case DrawableSpinner _:
                    colour = Color4.BlueViolet;
                    break;

                default:
                    colour = h.HitObject.Type == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
                    break;
            }

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new Drawable[]
            {
                topLine = new ExplosiveLine(colour),
                bottomLine = new ExplosiveLine(colour, true),
            };
        }

        protected override void Update()
        {
            base.Update();

            if (!topLine.IsAlive && !bottomLine.IsAlive)
                Expire(true);
        }

        private class ExplosiveLine : CompositeDrawable
        {
            private readonly bool rotated;

            public ExplosiveLine(Color4 colour, bool rotated = false)
            {
                this.rotated = rotated;

                Anchor = Anchor.Centre;
                Origin = Anchor.BottomCentre;
                Size = new Vector2(7, 40);
                InternalChild = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        Size = new Vector2(1, 34),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = colour
                    }
                }.WithEffect(new GlowEffect
                {
                    BlurSigma = new Vector2(3),
                    Colour = colour,
                    CacheDrawnEffect = true,
                    Strength = 10
                });

                if (rotated)
                    Rotation = -180;

                Y = (rotated ? 1 : -1) * SwingHitObject.DEFAULT_SIZE / 2;
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                this.FadeOut(250, Easing.Out);
                this.MoveToY((rotated ? 1 : -1) * (SwingHitObject.DEFAULT_SIZE / 2 + SwingPlayfield.FULL_SIZE.Y / 6), 250, Easing.Out);
                Expire(true);
            }
        }
    }
}
