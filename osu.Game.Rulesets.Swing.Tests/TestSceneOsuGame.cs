using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Swing.Tests
{
    [TestFixture]
    public class TestSceneOsuGame : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            OsuGame game = new OsuGame();
            game.SetHost(host);
            AddGame(game);
        }
    }
}
