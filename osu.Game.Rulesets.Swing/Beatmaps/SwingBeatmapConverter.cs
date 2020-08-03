using osu.Game.Beatmaps;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Swing.Beatmaps
{
    public class SwingBeatmapConverter : BeatmapConverter<SwingHitObject>
    {
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
                default:
                    if (strong)
                    {
                        yield return new Tap
                        {
                            StartTime = obj.StartTime,
                            Type = HitType.Up,
                            Samples = obj.Samples,
                            NewCombo = comboData?.NewCombo ?? false,
                            ComboOffset = comboData?.ComboOffset ?? 0,
                            IndexInBeatmap = index
                        };

                        yield return new Tap
                        {
                            StartTime = obj.StartTime,
                            Type = HitType.Down,
                            Samples = obj.Samples,
                            NewCombo = comboData?.NewCombo ?? false,
                            ComboOffset = comboData?.ComboOffset ?? 0,
                            IndexInBeatmap = index
                        };
                    }
                    else
                    {
                        yield return new Tap
                        {
                            StartTime = obj.StartTime,
                            Type = !isRim ? HitType.Up : HitType.Down,
                            Samples = obj.Samples,
                            NewCombo = comboData?.NewCombo ?? false,
                            ComboOffset = comboData?.ComboOffset ?? 0,
                            IndexInBeatmap = index
                        };
                    }
                    break;
            }
        }

        protected override Beatmap<SwingHitObject> CreateBeatmap() => new SwingBeatmap();
    }
}
