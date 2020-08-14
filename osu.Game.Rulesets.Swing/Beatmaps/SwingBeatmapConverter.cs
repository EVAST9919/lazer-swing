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

        public SwingBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        private int index = -1;

        protected override IEnumerable<SwingHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap)
        {
            var samples = obj.Samples;
            var comboData = obj as IHasCombo;

            if (comboData?.NewCombo ?? false)
            {
                index++;
            }

            static bool isRimDefinition(HitSampleInfo s) => s.Name == HitSampleInfo.HIT_CLAP || s.Name == HitSampleInfo.HIT_WHISTLE;
            bool isRim = samples.Any(isRimDefinition);

            bool strong = obj.Samples.Any(s => s.Name == HitSampleInfo.HIT_FINISH);

            switch (obj)
            {
                case IHasPathWithRepeats path:
                    return convertHitObject(obj, comboData, strong, isRim);

                case IHasDuration endTimeData:
                    {
                        double hitMultiplier = BeatmapDifficulty.DifficultyRange(beatmap.BeatmapInfo.BaseDifficulty.OverallDifficulty, 3, 5, 7.5) * spinner_hit_multiplier;

                        return new List<SwingHitObject>
                        {
                            new Spinner
                            {
                                StartTime = obj.StartTime,
                                Samples = obj.Samples,
                                Duration = endTimeData.Duration,
                                RequiredHits = (int)Math.Max(1, endTimeData.Duration / 1000 * hitMultiplier)
                            }
                        };
                    }

                default:
                    return convertHitObject(obj, comboData, strong, isRim);
            }
        }

        protected override Beatmap<SwingHitObject> CreateBeatmap() => new SwingBeatmap();

        private List<Tap> convertHitObject(HitObject obj, IHasCombo comboData, bool isStrong, bool isRim)
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
                        Samples = obj.Samples,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                        IndexInBeatmap = index
                    },
                    new Tap
                    {
                        StartTime = obj.StartTime,
                        Type = HitType.Down,
                        Samples = obj.Samples,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                        IndexInBeatmap = index
                    }
                });
            }
            else
            {
                taps.Add(new Tap
                {
                    StartTime = obj.StartTime,
                    Type = !isRim ? HitType.Up : HitType.Down,
                    Samples = obj.Samples,
                    NewCombo = comboData?.NewCombo ?? false,
                    ComboOffset = comboData?.ComboOffset ?? 0,
                    IndexInBeatmap = index
                });
            }

            return taps;
        }
    }
}
