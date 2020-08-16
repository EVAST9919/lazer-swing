using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces
{
    public class TapPiece : CircularContainer
    {
        [Resolved]
        private EditorClock editorClock { get; set; }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(SwingHitObject.DEFAULT_SIZE);
            Masking = true;
            BorderThickness = 4;
            BorderColour = colours.Yellow;
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.4f,
                Colour = colours.Yellow
            };
        }

        public void UpdateFrom(Tap tap)
        {
            if (tap.Type == HitType.Up)
            {
                Position = MathExtensions.GetRotatedPosition(new Vector2(SwingPlayfield.FULL_SIZE.Y - 150, 0), new Vector2(SwingPlayfield.FULL_SIZE.Y / 2 - 150, 0), (float)MathExtensions.Map(editorClock.CurrentTime, tap.StartTime - tap.TimePreempt, tap.StartTime + tap.TimePreempt, 0, -180));
            }
            else
            {
                Position = MathExtensions.GetRotatedPosition(new Vector2(SwingPlayfield.FULL_SIZE.Y - 150, SwingPlayfield.FULL_SIZE.Y), new Vector2(SwingPlayfield.FULL_SIZE.Y / 2 - 150, SwingPlayfield.FULL_SIZE.Y), (float)MathExtensions.Map(editorClock.CurrentTime, tap.StartTime - tap.TimePreempt, tap.StartTime + tap.TimePreempt, 0, 180));
            }
        }
    }
}
