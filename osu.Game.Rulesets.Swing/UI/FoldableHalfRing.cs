using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Swing.UI
{
    public class FoldableHalfRing : CompositeDrawable
    {
        private readonly Sprite ring;

        public FoldableHalfRing(RingState state)
        {
            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Height = 0.5f,
                Anchor = Anchor.Centre,
                Origin = Anchor.BottomCentre,
                Masking = true,
                Child = ring = new Sprite
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.Both,
                    Rotation = state == RingState.Closed ? 180 : 0
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            ring.Texture = textures.Get("ring");
        }

        public void Open(double time)
        {
            ring.RotateTo(180).Then().RotateTo(360, time, Easing.Out);
        }

        public void Close(double time)
        {
            ring.RotateTo(540, time, Easing.Out);
        }

        public void CloseBack(double time)
        {
            ring.RotateTo(180, time, Easing.Out);
        }
    }

    public enum RingState
    {
        Closed,
        Opened
    }
}
