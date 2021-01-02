using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Beatmaps
{
    public class SwingBeatmap : Beatmap<SwingHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int taps = HitObjects.Count(s => s is Tap);
            int holds = HitObjects.Count(s => s is Hold);
            int spinners = HitObjects.Count(s => s is Spinner);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Tap count",
                    Content = taps.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles)
                },
                new BeatmapStatistic
                {
                    Name = @"Hold count",
                    Content = holds.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Sliders)
                },
                new BeatmapStatistic
                {
                    Name = @"Spinner count",
                    Content = spinners.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Spinners)
                }
            };
        }
    }
}
