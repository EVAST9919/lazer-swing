using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
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
                    Name = @"Taps",
                    Content = taps.ToString(),
                    Icon = FontAwesome.Regular.Circle
                },
                new BeatmapStatistic
                {
                    Name = @"Holds",
                    Content = holds.ToString(),
                    Icon = FontAwesome.Regular.Circle
                },
                new BeatmapStatistic
                {
                    Name = @"Spinners",
                    Content = spinners.ToString(),
                    Icon = FontAwesome.Regular.Circle
                }
            };
        }
    }
}
