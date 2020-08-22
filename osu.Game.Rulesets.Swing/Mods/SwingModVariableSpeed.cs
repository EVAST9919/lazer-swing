using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModVariableSpeed : Mod, IApplicableToHitObject
    {
        public override string Name => "Variable Speed";

        public override string Acronym => "VS";

        public override double ScoreMultiplier => 1;

        public override string Description => "Adds taiko-like behavior";

        public override ModType Type => ModType.Fun;

        public void ApplyToHitObject(HitObject hitObject)
        {
            if (hitObject is SwingHitObject swingObject)
            {
                swingObject.UseSpeedMultiplier = true;
                swingObject.NestedHitObjects.ForEach(n =>
                {
                    ((SwingHitObject)n).UseSpeedMultiplier = true;
                });
            }
        }
    }
}
