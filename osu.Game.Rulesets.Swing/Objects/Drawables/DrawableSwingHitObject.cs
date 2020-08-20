using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public abstract class DrawableSwingHitObject : DrawableHitObject<SwingHitObject>, IKeyBindingHandler<SwingAction>
    {
        public bool Auto { get; set; }

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
        /// <summary>
        /// A list of keys which can result in hits for this HitObject.
        /// </summary>
        public SwingAction[] HitActions { get; protected set; }

        /// <summary>
        /// The action that caused this <see cref="DrawableHit"/> to be hit.
        /// </summary>
        public SwingAction? HitAction { get; protected set; }

        protected new T HitObject => (T)base.HitObject;

        protected virtual bool RequiresTimedUpdate { get; } = false;

        protected DrawableSwingHitObject(T hitObject)
            : base(hitObject)
        {
        }

        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;

        protected override void Update()
        {
            base.Update();

            if (RequiresTimedUpdate)
                Update(Time.Current);
        }

        protected virtual void Update(double currentTime)
        {
        }
    }
}
