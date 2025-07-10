using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Swing.Scoring
{
    public class SwingHitWindows : HitWindows
    {
        private static readonly DifficultyRange great_window = new(50, 35, 20);
        private static readonly DifficultyRange good_window = new(120, 80, 50);
        private static readonly DifficultyRange miss_window = new(135, 95, 70);

        public override bool IsHitResultAllowed(HitResult result)
        {
            switch (result)
            {
                case HitResult.Great:
                case HitResult.Good:
                case HitResult.Miss:
                    return true;
            }

            return false;
        }

        private double great;
        private double good;
        private double miss;

        public override void SetDifficulty(double difficulty)
        {
            great = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, great_window)) - 0.5;
            good = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, good_window)) - 0.5;
            miss = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, miss_window)) - 0.5;
        }

        public override double WindowFor(HitResult result)
        {
            switch (result)
            {
                case HitResult.Great:
                    return great;

                case HitResult.Good:
                    return good;

                case HitResult.Miss:
                    return miss;

                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}
