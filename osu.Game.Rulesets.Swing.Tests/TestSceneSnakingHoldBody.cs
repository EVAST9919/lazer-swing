using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Swing.Tests
{
    public class TestSceneSnakingHoldBody : OsuTestScene
    {
        protected override Ruleset CreateRuleset() => new SwingRuleset();

        private readonly SnakingHoldBody oldBody;
        private readonly PathSliderBody newBody;

        public TestSceneSnakingHoldBody()
        {
            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new TestContainer("old body", oldBody = new SnakingHoldBody
                        {
                            Anchor = Anchor.TopCentre,
                            Colour = Color4.DeepSkyBlue,
                        }),
                        new TestContainer("new body", newBody = new PathSliderBody
                        {
                            Colour = Color4.DeepSkyBlue
                        })
                        {
                            Anchor = Anchor.CentreLeft,
                        }
                    }
                }
            });
        }

        private class TestContainer : CompositeDrawable
        {
            public TestContainer(string text, Drawable child)
            {
                RelativeSizeAxes = Axes.Both;
                Height = 0.5f;
                InternalChildren = new Drawable[]
                {
                    new OsuSpriteText
                    {
                        Text = text,
                        Font = OsuFont.GetFont(size: 24, weight: FontWeight.Medium)
                    },
                    child.With(c =>
                    {
                        c.Margin = new MarginPadding { Top = 30 };
                    })
                };
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddSliderStep<float>("Unfold value", 0, 90, 0, newValue =>
            {
                oldBody.UnfoldToDegree(newValue);
                newBody.SetProgressDegree(newValue, 0);
            });
        }
    }
}
