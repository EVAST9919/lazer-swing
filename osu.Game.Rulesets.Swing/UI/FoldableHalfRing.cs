using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace osu.Game.Rulesets.Swing.UI
{
    public partial class FoldableHalfRing : CompositeDrawable
    {
        private readonly CircularProgress progress;

        public FoldableHalfRing(RingState state)
        {
            InternalChild = progress = new CircularProgress
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Current = { Value = 0 },
                Rotation = -90,
                InnerRadius = 0.01f
            };
        }

        public void Open(double duration)
        {
            progress.RotateTo(-90);
            progress.FillTo(0.5f, duration, Easing.Out);
        }

        public void Close(double duration)
        {
            progress.RotateTo(90, duration, Easing.Out);
            progress.FillTo(0f, duration, Easing.Out);
        }

        public void CloseBack(double duration)
        {
            progress.RotateTo(-90, duration, Easing.Out);
            progress.FillTo(0f, duration, Easing.Out);
        }
    }

    public enum RingState
    {
        Closed,
        Opened
    }
}
