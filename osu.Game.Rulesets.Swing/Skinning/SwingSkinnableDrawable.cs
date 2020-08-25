using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Swing.Skinning
{
    public abstract class SwingSkinnableDrawable : SkinReloadableDrawable
    {
        protected abstract string TextureName { get; }

        protected abstract Sprite SpriteToSkin { get; }

        [Resolved]
        private TextureStore textures { get; set; }

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            base.SkinChanged(skin, allowFallback);
            SpriteToSkin.Texture = skin.GetTexture(TextureName) ?? textures.Get(TextureName);
        }
    }
}
