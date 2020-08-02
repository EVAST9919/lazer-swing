using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Beatmaps
{
    public class SwingBeatmap : Beatmap<SwingHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics() => Array.Empty<BeatmapStatistic>();
    }
}
