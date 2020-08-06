using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHold : DrawableSwingHitObject<Hold>
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();
        public DrawableHoldHeadCircle HeadCircle => headContainer.Child;
        public DrawableHoldHeadSound HeadSound => headSoundContainer.Child;
        public DrawableHoldTailCircle TailCircle => tailContainer.Child;

        private readonly IHasPathWithRepeats path;

        private readonly Container<DrawableHoldHeadCircle> headContainer;
        private readonly Container<DrawableHoldHeadSound> headSoundContainer;
        private readonly Container<DrawableHoldTailCircle> tailContainer;
        private readonly Container<DrawableHoldTick> ticksContainer;
        private readonly Container<DrawableHoldRepeat> repeatsContainer;

        public DrawableHold(Hold h)
            : base(h)
        {
            path = h.Path;

            AddRangeInternal(new Drawable[]
            {
                headContainer = new Container<DrawableHoldHeadCircle>(),
                ticksContainer = new Container<DrawableHoldTick>(),
                repeatsContainer = new Container<DrawableHoldRepeat>(),
                tailContainer = new Container<DrawableHoldTailCircle>(),
                headSoundContainer = new Container<DrawableHoldHeadSound>(),
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableHoldHeadCircle head:
                    headContainer.Child = head;
                    break;

                case DrawableHoldHeadSound headSound:
                    headSoundContainer.Child = headSound;
                    break;

                case DrawableHoldTick tick:
                    ticksContainer.Add(tick);
                    break;

                case DrawableHoldRepeat repeat:
                    repeatsContainer.Add(repeat);
                    break;

                case DrawableHoldTailCircle tail:
                    tailContainer.Child = tail;
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            headContainer.Clear();
            headSoundContainer.Clear();
            ticksContainer.Clear();
            repeatsContainer.Clear();
            tailContainer.Clear();
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case HoldHeadCircle head:
                    return new DrawableHoldHeadCircle(head);

                case HoldHeadSound headSound:
                    return new DrawableHoldHeadSound(headSound);

                case HoldTick tick:
                    return new DrawableHoldTick(tick);

                case HoldRepeat repeat:
                    return new DrawableHoldRepeat(repeat);

                case HoldTailCircle tail:
                    return new DrawableHoldTailCircle(tail);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        private void updateType()
        {
            Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
        }

        public override bool OnPressed(SwingAction action) => false;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset < path.Duration)
                return;

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(path.Duration, true))
            {
                this.FadeOut(HitObject.TimePreempt / 3, Easing.OutQuint);
            }
        }

        protected override void Update()
        {
            base.Update();

            var isTracking = HeadCircle.IsTracking.Value;

            TailCircle.IsTracking = isTracking;

            foreach (var tick in ticksContainer)
                tick.IsTracking = isTracking;

            foreach (var repeat in repeatsContainer)
                repeat.IsTracking = isTracking;
        }
    }
}
