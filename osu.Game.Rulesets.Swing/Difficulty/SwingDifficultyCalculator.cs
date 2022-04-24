using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Difficulty
{
    public class SwingDifficultyCalculator : DifficultyCalculator
    {
        public SwingDifficultyCalculator(IRulesetInfo ruleset, IWorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, double clockRate)
        {
            int objectCount = beatmap.HitObjects.Count(h => h is SwingHitObject);
            double calculatedLength = beatmap.BeatmapInfo.Length / 1000 / clockRate;
            double starRating = 0.0;

            if (objectCount != 0 && calculatedLength != 0.0)
                starRating = objectCount / calculatedLength;

            return new DifficultyAttributes
            {
                StarRating = starRating,
                Mods = mods,
                MaxCombo = objectCount
            };
        }

        protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, double clockRate) => Enumerable.Empty<DifficultyHitObject>();

        protected override Skill[] CreateSkills(IBeatmap beatmap, Mod[] mods, double clockRate) => Array.Empty<Skill>();
    }
}
