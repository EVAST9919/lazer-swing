using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Framework.Graphics.Shapes;

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
            X = -150;
            Size = new Vector2(200, 100);
            Masking = true;
            InternalChildren = new Drawable[]
            {
                new CircularContainer
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.BottomCentre,
                    Size = new Vector2(200),
                    Masking = true,
                    BorderThickness = 4,
                    BorderColour = colours.Yellow,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colours.Yellow,
                        Alpha = 0.4f
                    }
                }
            };
        }
    }
}
