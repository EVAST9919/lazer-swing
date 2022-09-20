using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Swing.Beatmaps;

namespace osu.Game.Rulesets.Swing.Mods
{
    public class SwingModAlternate : Mod, IApplicableToBeatmapConverter
    {
        public override string Name => "Alternate";

        public override string Acronym => "AN";

        public override double ScoreMultiplier => 1;

        public override LocalisableString Description => "Left-right.";

        public override ModType Type => ModType.Conversion;

        public override IconUsage? Icon => FontAwesome.Solid.Sync;

        public void ApplyToBeatmapConverter(IBeatmapConverter beatmapConverter)
        {
            ((SwingBeatmapConverter)beatmapConverter).Alternate = true;
        }
    }
}
