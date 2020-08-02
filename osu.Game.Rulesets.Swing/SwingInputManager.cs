using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.Swing
{
    public class SwingInputManager : RulesetInputManager<SwingAction>
    {
        public SwingInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum SwingAction
    {
        [Description("Up Swing")]
        UpSwing,

        [Description("Up Swing (Additional)")]
        UpSwingAdditional,

        [Description("Down Swing")]
        DownSwing,

        [Description("Down Swing (Additional)")]
        DownSwingAdditional
    }
}
