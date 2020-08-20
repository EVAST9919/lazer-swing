using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class HoldBodyEnd : CompositeDrawable
    {
        private const int size = 25;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(size);
            InternalChild = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Width = 0.5f,
                Texture = textures.Get("hold-body-end")
            };
        }
    }
}
