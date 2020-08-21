using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldRepeat : DrawableSwingHitObject<HoldRepeat>
    {
        public bool Tracking { get; set; }

        private readonly Container contentContainer;
        private readonly Sprite sprite;

        public DrawableHoldRepeat(HoldRepeat h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                contentContainer = new Container
                {
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Rotation = -90,
                    Child = sprite = new Sprite
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(17)
                    }
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sprite.Texture = textures.Get("repeat");
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
            contentContainer.RotateTo(0, HitObject.TimePreempt);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= 0)
                ApplyResult(r => r.Type = Tracking ? r.Judgement.MaxResult : HitResult.Miss);
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 200, Easing.Out);
                    this.FadeOut(200, Easing.Out);
                    break;

                case ArmedState.Hit:
                    sprite.ScaleTo(1.3f, 200, Easing.Out);
                    this.FadeOut(200, Easing.Out);
                    break;
            }
        }

        public override bool OnPressed(SwingAction action) => false;
    }
}
