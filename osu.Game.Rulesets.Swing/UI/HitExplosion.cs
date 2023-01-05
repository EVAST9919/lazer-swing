using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Pooling;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Swing.UI
{
    public partial class HitExplosion : PoolableDrawable
    {
        private const float duration = 250;

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

        private Color4 colour;
        private JudgementResult result;

        public DrawableSwingHitObject JudgedObject { get; private set; }

        public void Apply(JudgementResult result, DrawableSwingHitObject judgetObject)
        {
            this.result = result;
            JudgedObject = judgetObject;

            switch (JudgedObject)
            {
                case DrawableSpinner _:
                    colour = Color4.BlueViolet;
                    break;
                default:
                    colour = judgetObject.HitObject.Type == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
                    break;
            }
        }

        protected override void PrepareForUse()
        {
            base.PrepareForUse();

            ClearTransforms(true);

            LifetimeStart = result.TimeAbsolute;
            LifetimeEnd = result.TimeAbsolute + duration;

            topLine.AccentColour = colour;
            bottomLine.AccentColour = colour;

            topLine.ResetAnimation();
            bottomLine.ResetAnimation();

            using(BeginAbsoluteSequence(result.TimeAbsolute))
            {
                topLine.Animate();
                bottomLine.Animate();
            }
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
                Alpha = 1f;
                Y = (rotated ? 1 : -1) * SwingHitObject.DEFAULT_SIZE / 2;
            }

            public void Animate()
            {
                this.FadeOut(duration, Easing.Out);
                this.MoveToY((rotated ? 1 : -1) * (SwingHitObject.DEFAULT_SIZE / 2 + SwingPlayfield.FULL_SIZE.Y / 6), duration, Easing.Out);
            }
        }
    }
}
