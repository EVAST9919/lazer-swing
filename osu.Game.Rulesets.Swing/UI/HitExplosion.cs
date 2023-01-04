using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Framework.Allocation;

namespace osu.Game.Rulesets.Swing.UI
{
    public partial class HitExplosion : DrawableJudgement
    {
        private ExplosiveLine topLine;
        private ExplosiveLine bottomLine;

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new Drawable[]
            {
                topLine = new ExplosiveLine(),
                bottomLine = new ExplosiveLine(true)
            };
        }

        protected override void PrepareForUse()
        {
            base.PrepareForUse();

            Color4 colour;

            switch (JudgedObject)
            {
                case DrawableSpinner _:
                    colour = Color4.BlueViolet;
                    break;
                default:
                    colour = ((DrawableSwingHitObject)JudgedObject).HitObject.Type == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
                    break;
            }

            topLine.AccentColour = colour;
            bottomLine.AccentColour = colour;
        }

        protected override void ApplyHitAnimations()
        {
            topLine.ResetAnimation();
            bottomLine.ResetAnimation();

            topLine.Animate();
            bottomLine.Animate();

            base.ApplyHitAnimations();
        }

        protected partial class ExplosiveLine : CompositeDrawable, IHasAccentColour
        {
            private Color4 accentColour;

            public Color4 AccentColour
            {
                get => accentColour;
                set
                {
                    accentColour = value;
                    bufferedContainer.EffectColour = value;
                    box.Colour = value;
                }
            }

            private readonly bool rotated;
            private readonly BufferedContainer bufferedContainer;
            private readonly Box box;

            public ExplosiveLine(bool rotated = false)
            {
                this.rotated = rotated;

                Anchor = Anchor.Centre;
                Origin = Anchor.BottomCentre;
                Size = new Vector2(7, 40);
                InternalChild = bufferedContainer = new BufferedContainer(cachedFrameBuffer: true)
                {
                    Size = new Vector2(7, 40),
                    BlurSigma = new Vector2(3),
                    EffectBlending = BlendingParameters.Additive,
                    DrawOriginal = true,
                    Child = box = new Box
                    {
                        Size = new Vector2(1, 34),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                };

                if (rotated)
                    Rotation = -180;
            }

            public void ResetAnimation()
            {
                ClearTransforms();

                Alpha = 1f;
                Y = (rotated ? 1 : -1) * SwingHitObject.DEFAULT_SIZE / 2;
            }

            public void Animate()
            {
                this.FadeOut(250, Easing.Out);
                this.MoveToY((rotated ? 1 : -1) * (SwingHitObject.DEFAULT_SIZE / 2 + SwingPlayfield.FULL_SIZE.Y / 6), 250, Easing.Out);
            }
        }
    }
}
