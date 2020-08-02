using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Swing.Configuration
{
    public class SwingRulesetConfigManager : RulesetConfigManager<SwingRulesetSetting>
    {
        public SwingRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            Set(SwingRulesetSetting.PlayfieldOrientation, PlayfieldOrientation.Taiko);
        }
    }

    public enum SwingRulesetSetting
    {
        PlayfieldOrientation
    }

    public enum PlayfieldOrientation
    {
        Taiko,
        Mania
    }
}
