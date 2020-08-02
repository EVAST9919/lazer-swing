using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Swing.Configuration;

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingSettingsSubsection : RulesetSettingsSubsection
    {
        protected override string Header => "Swing";

        public SwingSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (SwingRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                new SettingsEnumDropdown<PlayfieldOrientation>
                {
                    LabelText = "Playfield orientation",
                    Bindable = config.GetBindable<PlayfieldOrientation>(SwingRulesetSetting.PlayfieldOrientation)
                },
            };
        }
    }
}
