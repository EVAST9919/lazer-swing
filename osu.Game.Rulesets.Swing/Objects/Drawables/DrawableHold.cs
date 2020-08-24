using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHold : DrawableSwingHitObject<Hold>
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public readonly DrawableHoldBody Body;
        public readonly HoldBall Ball;

        private DrawableHoldTail tail => tailContainer.Child;
        private DrawableHoldHead head => headContainer.Child;

        private readonly Container<DrawableHoldHead> headContainer;
        private readonly Container<DrawableHoldTail> tailContainer;
        private readonly Container<DrawableHoldTick> ticksContainer;

        public DrawableHold(Hold h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                Body = new DrawableHoldBody(h),
                ticksContainer = new Container<DrawableHoldTick>(),
                tailContainer = new Container<DrawableHoldTail>(),
                headContainer = new Container<DrawableHoldHead>(),
                Ball = new HoldBall()
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

                case HoldTail tail:
                    return new DrawableHoldTail(tail);

                case HoldTick tick:
                    return new DrawableHoldTick(tick);
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

                case DrawableHoldTail tail:
                    tailContainer.Child = tail;
                    break;

                case DrawableHoldTick tick:
                    ticksContainer.Add(tick);
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();

            headContainer.Clear();
            tailContainer.Clear();
            ticksContainer.Clear();
        }

        private bool tracking => (HitAction != null || Auto) && Time.Current < HitObject.EndTime;

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
            Ball.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
            Ball.RotateTo(0, HitObject.TimePreempt);
            Body.StartTransform();
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (Time.Current > HitObject.StartTime)
            {
                foreach (var t in ticksContainer)
                    t.Tracking = tracking;

                Ball.Tracking.Value = tracking;
            }

            if (Time.Current < HitObject.EndTime)
                return;

            bool allTicksMisses = true;
            bool allTicksPerfect = true;

            if (ticksContainer.Count == 0)
            {
                allTicksPerfect = true;
                allTicksMisses = false;
            }
            else
            {
                foreach (var t in ticksContainer)
                {
                    if (t.Result.Type == HitResult.Miss)
                    {
                        allTicksPerfect = false;
                        break;
                    }
                }

                if (!allTicksPerfect)
                {
                    foreach (var t in ticksContainer)
                    {
                        if (t.Result.Type == HitResult.Great)
                        {
                            allTicksMisses = false;
                            break;
                        }
                    }
                }
                else
                    allTicksMisses = false;
            }

            var headResult = head.Result.Type;

            bool headIsPerfect = headResult == HitResult.Great;
            bool headIsMiss = headResult == HitResult.Miss;

            if (allTicksPerfect && headIsPerfect)
                tail.TriggerResult(HitResult.Great);
            else if ((allTicksMisses || ticksContainer.Count == 0) && headIsMiss)
                tail.TriggerResult(HitResult.Miss);
            else
                tail.TriggerResult(HitResult.Good);

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(HitObject.Duration, true))
            {
                this.FadeOut(300, Easing.OutQuint);
                Ball.EndAnimation(300);
            }
        }
    }
}
