using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Swing.Tests
{
    public class TestSceneSmoothPathRotation : OsuTestScene
    {
        private readonly SmoothPath path;

        public TestSceneSmoothPathRotation()
        {
            Add(path = new SmoothPath
            {
                Anchor = Anchor.Centre,
                Vertices = new[]
                {
                    Vector2.Zero,
                    new Vector2(100, 100)
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("start", () => path.Spin(1000, RotationDirection.Clockwise));
            AddStep("stop", () => path.ClearTransforms());
        }
    }
}
