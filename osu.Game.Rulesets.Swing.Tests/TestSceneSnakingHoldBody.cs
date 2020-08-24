using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Swing.Tests
{
    public class TestSceneSnakingHoldBody : OsuTestScene
    {
        protected override Ruleset CreateRuleset() => new SwingRuleset();

        private readonly SnakingHoldBody body;

        public TestSceneSnakingHoldBody()
        {
            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray
                },
                body = new SnakingHoldBody
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(-100),
                    Colour = Color4.DeepSkyBlue
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddSliderStep<float>("Unfold value", 0, 90, 0, newValue => body.UnfoldToDegree(newValue));
            AddSliderStep<float>("Rotation", 0, 90, 0, newValue => body.Rotation = newValue);
        }
    }
}
