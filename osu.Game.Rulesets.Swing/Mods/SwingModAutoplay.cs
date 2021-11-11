using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Replays;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Game.Scoring;
using System.Collections.Generic;
using osu.Game.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModAutoplay : ModAutoplay, IApplicableToDrawableHitObject
    {
        public override Score CreateReplayScore(IBeatmap beatmap, IReadOnlyList<Mod> mods) => new Score
        {
            ScoreInfo = new ScoreInfo { User = new APIUser { Username = "auto" } },
            Replay = new SwingAutoGenerator(beatmap).Generate(),
        };

        public void ApplyToDrawableHitObject(DrawableHitObject drawable)
        {
            if (drawable is DrawableSwingHitObject swingObject)
            {
                swingObject.Auto = true;

                foreach (DrawableSwingHitObject nested in swingObject.NestedHitObjects)
                    nested.Auto = true;
            }
        }
    }
}
