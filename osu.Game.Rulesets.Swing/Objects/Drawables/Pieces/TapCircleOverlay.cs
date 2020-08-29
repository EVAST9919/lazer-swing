using osu.Framework.Graphics;
using osu.Game.Rulesets.Swing.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class TapCircleOverlay : SwingSkinnableDrawable
    {
        protected override string TextureName => "tap-overlay";

        public TapCircleOverlay()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE + 20);
        }
    }
}
