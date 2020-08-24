using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class HoldBodyEnd : Sprite
    {
        private const int size = 50;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Origin = Anchor.CentreLeft;
            Size = new Vector2(size / 2, size);
            Texture = textures.Get("hold-body-end");
        }
    }
}
