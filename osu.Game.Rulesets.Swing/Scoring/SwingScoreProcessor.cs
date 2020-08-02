using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Scoring
{
    public class SwingScoreProcessor : ScoreProcessor
    {
        public override HitWindows CreateHitWindows() => new SwingHitWindows();
    }
}
