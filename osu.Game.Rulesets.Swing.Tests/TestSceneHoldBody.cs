using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Tests
{
    public partial class TestSceneHoldBody : RulesetTestScene
    {
        private readonly PathSliderBody adjustableBody;

        public TestSceneHoldBody()
        {
            Add(new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Margin = new MarginPadding { Top = 30 },
                Children = new Drawable[]
                {
                    new PathSliderBody
                    {
                        AccentColour = Color4.DeepSkyBlue,
                        HeadAngle = 90f,
                        TailAngle = 0f
                    },
                    adjustableBody = new PathSliderBody
                    {
                        AccentColour = Color4.Red
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddSliderStep("start degree", 0f, 90f, 90f, val => adjustableBody.HeadAngle = val);
            AddSliderStep("end degree", 0f, 90f, 0f, val => adjustableBody.TailAngle = val);
        }
    }
}
