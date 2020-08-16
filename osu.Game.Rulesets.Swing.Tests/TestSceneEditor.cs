using NUnit.Framework;
using osu.Game.Tests.Visual;
namespace osu.Game.Rulesets.Swing.Tests
{
    [TestFixture]
    public class TestSceneEditor : EditorTestScene
    {
        protected override Ruleset CreateEditorRuleset() => new SwingRuleset();
    }
}
