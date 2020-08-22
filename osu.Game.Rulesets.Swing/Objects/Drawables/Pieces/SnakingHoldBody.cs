using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class SnakingHoldBody : CompositeDrawable
    {
        private const float size = 281;

        public double UnfoldDegree
        {
            get => sprite.Rotation;
            set => sprite.Rotation = (float)value;
        }

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
    }
}
