using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHold : DrawableSwingHitObject<Hold>
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public readonly DrawableHoldBody Body;

        private readonly Container<DrawableHoldHead> headContainer;
        private readonly Container<DrawableHoldTick> ticksContainer;
        private readonly Container<DrawableHoldRepeat> repeatsContainer;

        public DrawableHold(Hold h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                Body = new DrawableHoldBody(h),
                ticksContainer = new Container<DrawableHoldTick>(),
                repeatsContainer = new Container<DrawableHoldRepeat>(),
                headContainer = new Container<DrawableHoldHead>()
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            HitActions =
                HitObject.Type == HitType.Up
                    ? new[] { SwingAction.UpSwing, SwingAction.UpSwingAdditional }
                    : new[] { SwingAction.DownSwing, SwingAction.DownSwingAdditional };

            Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Scale = Type.Value == HitType.Up ? Vector2.One : new Vector2(1, -1);
        }

        protected override void LoadSamples()
        {
            // Making this empty so the object itself will not produce any sounds
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case HoldHead head:
                    return new DrawableHoldHead(head);

                case HoldTick tick:
                    return new DrawableHoldTick(tick);

                case HoldRepeat repeat:
                    return new DrawableHoldRepeat(repeat);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableHoldHead head:
                    headContainer.Child = head;
                    break;

                case DrawableHoldTick tick:
                    ticksContainer.Add(tick);
                    break;

                case DrawableHoldRepeat repeat:
                    repeatsContainer.Add(repeat);
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();

            headContainer.Clear();
            ticksContainer.Clear();
            repeatsContainer.Clear();
        }

        private bool tracking => HitAction != null || Auto;

        public override bool OnPressed(SwingAction action)
        {
            if (HitActions.Contains(action))
                HitAction = action;

            return false;
        }

        public override void OnReleased(SwingAction action)
        {
            if (action == HitAction)
                HitAction = null;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            Body.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
        }

        protected override void Update()
        {
            base.Update();

            foreach (var t in ticksContainer)
                t.Tracking = tracking;

            foreach (var r in repeatsContainer)
                r.Tracking = tracking;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (Time.Current < HitObject.EndTime)
                return;

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(HitObject.Duration, true))
                this.FadeOut(300, Easing.OutQuint);
        }
    }
}
