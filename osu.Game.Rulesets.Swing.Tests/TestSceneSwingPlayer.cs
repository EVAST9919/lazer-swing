using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Swing.Tests
{
    [TestFixture]
    public class TestSceneSwingPlayer : PlayerTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new SwingRuleset();
    }
}
