using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.UI
{
    public partial class Ring : CircularContainer
    {
        public Ring(float thickness = 2)
        {
            Masking = true;
            BorderColour = Color4.White;
            BorderThickness = thickness;
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Alpha = 0,
                AlwaysPresent = true
            };
        }
    }
}
