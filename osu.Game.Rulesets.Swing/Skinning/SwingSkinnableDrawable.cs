using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Swing.Skinning
{
    public abstract class SwingSkinnableDrawable : SkinReloadableDrawable
    {
        protected abstract string TextureName { get; }

        [Resolved]
        private TextureStore textures { get; set; }

        private readonly Sprite sprite;

        public SwingSkinnableDrawable()
        {
            InternalChild = sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            };
        }

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            base.SkinChanged(skin, allowFallback);
            sprite.Texture = skin.GetTexture(TextureName) ?? textures.Get(TextureName);
        }
    }
}
