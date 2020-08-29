using osu.Framework.Graphics;
using osu.Game.Rulesets.Swing.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class TickElement : SwingSkinnableDrawable
    {
        protected override string TextureName => "tick";

        public TickElement()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(11);
        }
    }
}
