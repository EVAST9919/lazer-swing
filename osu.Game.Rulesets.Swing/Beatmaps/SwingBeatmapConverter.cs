using osu.Game.Beatmaps;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Audio;
using System;

namespace osu.Game.Rulesets.Swing.Beatmaps
{
    public class SwingBeatmapConverter : BeatmapConverter<SwingHitObject>
    {
        private const float spinner_hit_multiplier = 1.65f;

        public bool ConvertSliders { get; set; } = true;

        public bool Alternate { get; set; }

        public SwingBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => true;

        private bool isTop;

        protected override IEnumerable<SwingHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap)
        {
            var samples = obj.Samples;

            static bool isRimDefinition(HitSampleInfo s) => s.Name == HitSampleInfo.HIT_CLAP || s.Name == HitSampleInfo.HIT_WHISTLE;

            bool isRim = Alternate ? isTop = !isTop : samples.Any(isRimDefinition);
            bool strong = Alternate ? false : obj.Samples.Any(s => s.Name == HitSampleInfo.HIT_FINISH);

            switch (obj)
            {
                case IHasPathWithRepeats path:
                    if (ConvertSliders)
                        return new List<SwingHitObject>
                        {
                            new Hold
                            {
                                Path = path,
                                StartTime = obj.StartTime,
                                Samples = samples,
                                Duration = path.Duration,
                                Type = isRim ? HitType.Down : HitType.Up
                            }
                        };
                    else
                    {
                        return convertHitObject(obj, samples, strong, isRim);
                    }

                case IHasDuration endTimeData:
                    {
                        double hitMultiplier = BeatmapDifficulty.DifficultyRange(beatmap.BeatmapInfo.BaseDifficulty.OverallDifficulty, 3, 5, 7.5) * spinner_hit_multiplier;

                        return new List<SwingHitObject>
                        {
                            new Spinner
                            {
                                StartTime = obj.StartTime,
                                Samples = samples,
                                Duration = endTimeData.Duration,
                                RequiredHits = (int)Math.Max(1, endTimeData.Duration / 1000 * hitMultiplier)
                            }
                        };
                    }

                default:
                    return convertHitObject(obj, samples, strong, isRim);
            }
        }

        protected override Beatmap<SwingHitObject> CreateBeatmap() => new SwingBeatmap();

        private List<Tap> convertHitObject(HitObject obj, IList<HitSampleInfo> samples, bool isStrong, bool isRim)
        {
            var taps = new List<Tap>();

            if (isStrong)
            {
                taps.AddRange(new[]
                {
                    new Tap
                    {
                        StartTime = obj.StartTime,
                        Type = HitType.Up,
                        Samples = samples
                    },
                    new Tap
                    {
                        StartTime = obj.StartTime,
                        Type = HitType.Down,
                        Samples = samples
                    }
                });
            }
            else
            {
                taps.Add(new Tap
                {
                    StartTime = obj.StartTime,
                    Type = isRim ? HitType.Down : HitType.Up,
                    Samples = samples
                });
            }

            return taps;
        }
    }
}
