using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Mods;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModDifficultyAdjust : ModDifficultyAdjust
    {
        [SettingSource("Approach Rate", "Override a beatmap's set AR.", LAST_SETTING_ORDER + 1)]
        public BindableNumber<float> ApproachRate { get; } = new BindableFloat
        {
            Precision = 0.1f,
            MinValue = 0,
            MaxValue = 10,
            Default = 5,
            Value = 5,
        };

        public override string SettingDescription
        {
            get
            {
                string approachRate = ApproachRate.IsDefault ? string.Empty : $"AR {ApproachRate.Value:N1}";

                return string.Join(", ", new[]
                {
                    base.SettingDescription,
                    approachRate
                }.Where(s => !string.IsNullOrEmpty(s)));
            }
        }

        protected override void TransferSettings(BeatmapDifficulty difficulty)
        {
            base.TransferSettings(difficulty);

            TransferSetting(ApproachRate, difficulty.ApproachRate);
        }

        protected override void ApplySettings(BeatmapDifficulty difficulty)
        {
            base.ApplySettings(difficulty);

            difficulty.ApproachRate = ApproachRate.Value;
        }
    }
}
