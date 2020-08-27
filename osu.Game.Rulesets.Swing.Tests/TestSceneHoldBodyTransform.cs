using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Tests
{
    public class TestSceneHoldBodyTransform : OsuTestScene
    {
        private readonly PathSliderBody body;
        private readonly PathSliderBody adjustableBody;

        private readonly BindableDouble startDegree = new BindableDouble();
        private readonly BindableDouble endDegree = new BindableDouble();

        public TestSceneHoldBodyTransform()
        {
            Add(new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Margin = new MarginPadding { Top = 30 },
                Children = new Drawable[]
                {
                    body = new PathSliderBody
                    {
                        AccentColour = Color4.DeepSkyBlue
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

            body.SetProgressDegree(90, 0);

            startDegree.BindValueChanged(_ => updatePath());
            endDegree.BindValueChanged(_ => updatePath());

            AddSliderStep<double>("start degree", 0, 90, 90, val => startDegree.Value = val);
            AddSliderStep<double>("end degree", 0, 90, 0, val => endDegree.Value = val);
        }

        private void updatePath()
        {
            adjustableBody.SetProgressDegree(startDegree.Value, endDegree.Value);
        }
    }
}
