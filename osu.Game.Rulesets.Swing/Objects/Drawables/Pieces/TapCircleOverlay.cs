using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Swing.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class TapCircleOverlay : SwingSkinnableDrawable
    {
        protected override string TextureName => "tap-overlay";

        protected override Sprite SpriteToSkin => sprite;

        private readonly Sprite sprite;

        public TapCircleOverlay()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE + 20);
            InternalChild = sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
