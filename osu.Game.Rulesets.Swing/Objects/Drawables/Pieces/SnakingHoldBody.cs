using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class SnakingHoldBody : CompositeDrawable
    {
        private const float size = 281;

        private readonly Sprite sprite;

        public SnakingHoldBody()
        {
            Anchor = Anchor.TopCentre;
            Size = new Vector2(size);
            Masking = true;
            InternalChild = sprite = new Sprite
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomLeft,
                RelativeSizeAxes = Axes.Both
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sprite.Texture = textures.Get("hold-body");
        }

        public TransformSequence<Sprite> UnfoldToDegree(float newValue, double duration = 0, Easing easing = Easing.None)
            => sprite.RotateTo(newValue, duration, easing);
    }
}
