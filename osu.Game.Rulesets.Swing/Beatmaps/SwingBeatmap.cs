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
            int spinners = HitObjects.Count(s => s is Spinner);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Tap Count",
                    Content = taps.ToString(),
                    Icon = FontAwesome.Regular.Circle
                },
                new BeatmapStatistic
                {
                    Name = @"Spinner Count",
                    Content = spinners.ToString(),
                    Icon = FontAwesome.Regular.Circle
                }
            };
        }
    }
}
