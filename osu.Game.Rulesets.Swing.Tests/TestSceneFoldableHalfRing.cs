using osu.Game.Rulesets.Swing.UI;
using osu.Framework.Graphics;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Swing.Tests
{
    public class TestSceneFoldableHalfRing : OsuTestScene
    {
        protected override Ruleset CreateRuleset() => new SwingRuleset();

        private readonly FoldableHalfRing ring;

        public TestSceneFoldableHalfRing()
        {
            Add(ring = new FoldableHalfRing(RingState.Closed)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(200)
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddStep("Open", () => ring.Open(500));
            AddStep("Close", () => ring.Close(500));
            AddStep("Close back", () => ring.CloseBack(500));
        }
    }
}
