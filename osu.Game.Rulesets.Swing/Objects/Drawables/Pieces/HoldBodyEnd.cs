using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Swing.Skinning;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class HoldBodyEnd : SwingSkinnableDrawable
    {
        private const int size = 50;

        protected override string TextureName => "hold-body-end";

        protected override Sprite SpriteToSkin => sprite;

        private readonly Sprite sprite;

        public HoldBodyEnd()
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.CentreLeft;
            InternalChild = sprite = new Sprite
            {
                Size = new Vector2(size / 2, size)
            };
        }
    }
}
