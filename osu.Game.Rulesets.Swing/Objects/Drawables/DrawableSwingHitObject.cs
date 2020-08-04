using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing;
using osu.Game.Rulesets.Swing.Objects;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableSwingHitObject : DrawableHitObject<SwingHitObject>, IKeyBindingHandler<SwingAction>
    {
        protected DrawableSwingHitObject(SwingHitObject hitObject)
            : base(hitObject)
        {
        }

        public abstract bool OnPressed(SwingAction action);

        public virtual void OnReleased(SwingAction action)
        {
        }
    }

    public abstract class DrawableSwingHitObject<T> : DrawableSwingHitObject
        where T : SwingHitObject
    {
        protected override Color4 GetComboColour(IReadOnlyList<Color4> comboColours) =>
            comboColours[(HitObject.IndexInBeatmap + 1) % comboColours.Count];

        /// <summary>
        /// A list of keys which can result in hits for this HitObject.
        /// </summary>
        public SwingAction[] HitActions { get; protected set; }

        /// <summary>
        /// The action that caused this <see cref="DrawableHit"/> to be hit.
        /// </summary>
        public SwingAction? HitAction { get; protected set; }

        protected DrawableSwingHitObject(T hitObject)
            : base(hitObject)
        {
        }

        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;

        protected override void UpdateStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Idle:
                    // Manually set to reduce the number of future alive objects to a bare minimum.
                    LifetimeStart = HitObject.StartTime - HitObject.TimePreempt;
                    break;
            }
        }
    }
}
