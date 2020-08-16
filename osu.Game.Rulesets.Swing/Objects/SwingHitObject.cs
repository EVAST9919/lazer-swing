using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects
{
    public abstract class SwingHitObject : HitObject, IHasPosition
    {
        public double TimePreempt;

        public bool UseSpeedMultiplier
        {
            set
            {
                if (value)
                {
                    var adjustedSM = MathExtensions.Map(speedMultiplier, 0, 10, MathExtensions.Map(0.2, 0, 1, 0.2, 1), MathExtensions.Map(0.2, 0, 1, 1, 10));
                    TimePreempt /= adjustedSM;
                }
            }
        }

        private double speedMultiplier;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)BeatmapDifficulty.DifficultyRange(difficulty.ApproachRate, 1800, 1200, 450);
            speedMultiplier = controlPointInfo.DifficultyPointAt(StartTime).SpeedMultiplier;
        }

        protected override HitWindows CreateHitWindows() => new SwingHitWindows();

        public const float DEFAULT_SIZE = 50;

        public readonly Bindable<HitType> TypeBindable = new Bindable<HitType>();

        /// <summary>
        /// The <see cref="HitType"/> that actuates this <see cref="Hit"/>.
        /// </summary>
        public HitType Type
        {
            get => TypeBindable.Value;
            set => TypeBindable.Value = value;
        }

        public Vector2 Position => new Vector2(X, Y);

        public float X { get; set; }

        public float Y { get; set; }
    }
}
