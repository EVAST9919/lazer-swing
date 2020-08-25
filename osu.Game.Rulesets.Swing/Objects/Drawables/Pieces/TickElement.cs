using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Swing.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class TickElement : SwingSkinnableDrawable
    {
        protected override string TextureName => "tick";

        protected override Sprite SpriteToSkin => sprite;

        private readonly Sprite sprite;

        public TickElement()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(11);
            InternalChild = sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
