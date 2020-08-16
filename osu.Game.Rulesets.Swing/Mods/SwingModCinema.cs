using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Replays;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Game.Scoring;
using osu.Game.Users;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModCinema : ModCinema<SwingHitObject>, IApplicableToDrawableHitObjects
    {
        public override Score CreateReplayScore(IBeatmap beatmap) => new Score
        {
            ScoreInfo = new ScoreInfo { User = new User { Username = "auto" } },
            Replay = new SwingAutoGenerator(beatmap).Generate(),
        };

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            foreach (var d in drawables.OfType<DrawableSwingHitObject>())
            {
                d.Auto = true;
                foreach (DrawableSwingHitObject nested in d.NestedHitObjects)
                    nested.Auto = true;
            }
        }
    }
}
