using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osu.Framework.Graphics;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces
{
    public class SpinnerPiece : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.BottomCentre;
            Rotation = 90;
            Size = new Vector2(200, 100);
            InternalChild = new BasicHalfRing
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Colour = colours.Yellow
            };
        }
    }
}
