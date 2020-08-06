using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class DrawableTapCircle : Container
    {
        public Box Circle { get; private set; }

        public DrawableTapCircle()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE);
            Child = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderThickness = 4,
                BorderColour = Color4.White,
                Child = Circle = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                EdgeEffect = new EdgeEffectParameters
                {
                    Colour = Color4.Black.Opacity(0.25f),
                    Type = EdgeEffectType.Shadow,
                    Radius = 10
                }
            };
        }
    }
}
