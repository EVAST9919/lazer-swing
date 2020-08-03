using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Replays;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModAutoplay : ModAutoplay<SwingHitObject>
    {
        public override Score CreateReplayScore(IBeatmap beatmap) => new Score
        {
            ScoreInfo = new ScoreInfo { User = new User { Username = "auto" } },
            Replay = new SwingAutoGenerator(beatmap).Generate(),
        };
    }
}
