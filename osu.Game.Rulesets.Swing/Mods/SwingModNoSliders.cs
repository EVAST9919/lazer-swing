using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Swing.Beatmaps;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModNoSliders : Mod, IApplicableToBeatmapConverter
    {
        public override string Name => "No Sliders";

        public override string Acronym => "NS";

        public override double ScoreMultiplier => 1;

        public override string Description => "Only circles matter.";

        public override ModType Type => ModType.Conversion;

        public void ApplyToBeatmapConverter(IBeatmapConverter beatmapConverter)
        {
            ((SwingBeatmapConverter)beatmapConverter).ConvertSliders = false;
        }
    }
}
