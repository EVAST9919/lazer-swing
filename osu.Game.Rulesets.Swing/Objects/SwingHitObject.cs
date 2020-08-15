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
    public abstract class SwingHitObject : HitObject, IHasPosition, IHasComboInformation
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

        public readonly Bindable<Vector2> PositionBindable = new Bindable<Vector2>();

        public virtual Vector2 Position
        {
            get => PositionBindable.Value;
            set => PositionBindable.Value = value;
        }

        public float X => Position.X;
        public float Y => Position.Y;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)BeatmapDifficulty.DifficultyRange(difficulty.ApproachRate, 1800, 1200, 450);
            speedMultiplier = controlPointInfo.DifficultyPointAt(StartTime).SpeedMultiplier;
        }

        protected override HitWindows CreateHitWindows() => new SwingHitWindows();

        public virtual bool NewCombo { get; set; }

        public int ComboOffset { get; set; }

        public Bindable<int> IndexInCurrentComboBindable { get; } = new Bindable<int>();

        public int IndexInCurrentCombo
        {
            get => IndexInCurrentComboBindable.Value;
            set => IndexInCurrentComboBindable.Value = value;
        }

        public int IndexInBeatmap { get; set; }

        public Bindable<int> ComboIndexBindable { get; } = new Bindable<int>();

        public int ComboIndex
        {
            get => ComboIndexBindable.Value;
            set => ComboIndexBindable.Value = value;
        }

        public Bindable<bool> LastInComboBindable { get; } = new Bindable<bool>();

        public virtual bool LastInCombo
        {
            get => LastInComboBindable.Value;
            set => LastInComboBindable.Value = value;
        }

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
    }
}
