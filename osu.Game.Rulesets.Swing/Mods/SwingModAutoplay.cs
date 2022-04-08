using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Replays;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModAutoplay : ModAutoplay, IApplicableToDrawableHitObject
    {
        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new ModReplayData(new SwingAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "Autoplay" });

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
