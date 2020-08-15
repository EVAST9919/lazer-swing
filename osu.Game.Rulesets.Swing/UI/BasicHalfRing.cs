using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Swing.UI
{
    public class BasicHalfRing : CompositeDrawable
    {
        private readonly Sprite sprite;

        public BasicHalfRing()
        {
            InternalChild = sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sprite.Texture = textures.Get("ring");
        }
    }
}
