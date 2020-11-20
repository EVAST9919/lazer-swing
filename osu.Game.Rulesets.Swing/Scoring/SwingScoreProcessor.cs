using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Scoring
{
    public class SwingScoreProcessor : ScoreProcessor
    {
        protected override double DefaultAccuracyPortion => 0.75;

        protected override double DefaultComboPortion => 0.25;
    }
}
