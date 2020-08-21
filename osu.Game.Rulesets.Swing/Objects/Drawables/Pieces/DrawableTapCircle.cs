using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
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

        public readonly Circle Circle;

        public DrawableTapCircle()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE);
            Add(Circle = new Circle
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Add(new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(SwingHitObject.DEFAULT_SIZE + 20),
                Texture = textures.Get("tap-overlay")
            });
        }
    }
}
