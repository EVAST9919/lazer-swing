using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Swing.Objects
{
    public abstract class SwingHitObject : HitObject, IHasPosition
    {
        public double TimePreempt;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)BeatmapDifficulty.DifficultyRange(difficulty.ApproachRate, 1800, 1200, 450);
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
