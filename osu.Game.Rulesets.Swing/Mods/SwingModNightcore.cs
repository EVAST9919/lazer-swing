using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModNightcore : ModNightcore<SwingHitObject>
    {
        public override double ScoreMultiplier => 1.1f;
    }
}
