using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class DrawableTapCircle : Container
    {
        public new Color4 Colour
        {
            get => Circle.Colour;
            set => Circle.Colour = value;
        }

        public readonly TapCircle Circle;

        public DrawableTapCircle()
        {
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                Circle = new TapCircle
                {
                    Anchor = Anchor.Centre
                },
                new TapCircleOverlay
                {
                    Anchor = Anchor.Centre
                }
            };
        }
    }
}
