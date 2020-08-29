using osu.Game.Rulesets.Swing.Skinning;
using osuTK;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class TapCircle : SwingSkinnableDrawable
    {
        protected override string TextureName => "tap";

        public TapCircle()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE);
        }
    }
}
