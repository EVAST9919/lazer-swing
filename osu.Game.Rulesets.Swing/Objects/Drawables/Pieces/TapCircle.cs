using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Swing.Skinning;
using osuTK;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class TapCircle : SwingSkinnableDrawable
    {
        protected override string TextureName => "tap";

        protected override Sprite SpriteToSkin => sprite;

        private readonly Sprite sprite;

        public TapCircle()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE);
            InternalChild = sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
