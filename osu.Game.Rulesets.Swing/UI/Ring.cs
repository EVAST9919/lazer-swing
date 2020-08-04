using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.UI
{
    public class Ring : CircularContainer
    {
        public Ring()
        {
            Masking = true;
            BorderColour = Color4.White;
            BorderThickness = 2;
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Alpha = 0,
                AlwaysPresent = true
            };
        }
    }
}
