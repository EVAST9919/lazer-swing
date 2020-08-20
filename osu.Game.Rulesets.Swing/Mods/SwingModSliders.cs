using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Swing.Beatmaps;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModSliders : Mod, IApplicableToBeatmapConverter
    {
        public override string Name => "Sliders";

        public override string Acronym => "SL";

        public override double ScoreMultiplier => 1;

        public override string Description => "Adds proper sliders conversion";

        public override ModType Type => ModType.Conversion;

        public void ApplyToBeatmapConverter(IBeatmapConverter beatmapConverter)
        {
            ((SwingBeatmapConverter)beatmapConverter).ConvertSliders = true;
        }
    }
}
