using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModVariableSpeed : Mod, IApplicableToHitObject
    {
        public override string Name => "Variable Speed";

        public override string Acronym => "VS";

        public override double ScoreMultiplier => 1.5;

        public override string Description => "Adds taiko-like behavior";

        public override ModType Type => ModType.DifficultyIncrease;

        public void ApplyToHitObject(HitObject hitObject)
        {
            if (hitObject is SwingHitObject)
            {
                ((SwingHitObject)hitObject).UseSpeedMultiplier = true;
            }
        }
    }
}
