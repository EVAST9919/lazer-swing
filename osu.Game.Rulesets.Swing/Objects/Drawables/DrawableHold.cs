using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osuTK;
using osuTK.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHold : DrawableSwingHitObject<Hold>
    {
        protected override bool RequiresTimedUpdate => true;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public readonly PathSliderBody Body;
        public readonly HoldBall Ball;

        private DrawableHoldTail tail => tailContainer.Child;
        private DrawableHoldHead head => headContainer.Child;

        private readonly Container<DrawableHoldHead> headContainer;
        private readonly Container<DrawableHoldTail> tailContainer;

        private readonly double unfoldTime;
        private readonly double foldTime;

        public DrawableHold(Hold h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                Body = new PathSliderBody(),
                tailContainer = new Container<DrawableHoldTail>(),
                headContainer = new Container<DrawableHoldHead>(),
                Ball = new HoldBall()
            });

            unfoldTime = h.StartTime - h.TimePreempt;
            foldTime = unfoldTime + h.Duration;
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
            Body.AccentColour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red;
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
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();

            headContainer.Clear();
            tailContainer.Clear();
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
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            var currentTime = Time.Current;

            if (currentTime > HitObject.StartTime)
            {
                Ball.Tracking.Value = tracking;
            }

            if (currentTime < HitObject.EndTime)
                return;

            var headResult = head.Result.Type;

            bool headIsPerfect = headResult == HitResult.Great;
            bool headIsMiss = headResult == HitResult.Miss;

            if (headIsPerfect)
                tail.TriggerResult(HitResult.Great);
            else if (headIsMiss)
                tail.TriggerResult(HitResult.Miss);

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            base.UpdateHitStateTransforms(state);

            if (tail.Result.Type == HitResult.Miss)
            {
                Ball.FadeColour(Color4.Red, 50, Easing.OutQuint);
                Body.FadeColour(Color4.Red, 50, Easing.OutQuint);
            }

            this.FadeOut(300, Easing.OutQuint);
            Ball.Ring.ScaleTo(2f, 300, Easing.Out);
        }

        protected override void Update(double currentTime)
        {
            base.Update(currentTime);

            var tailValue = MathExtensions.Map(currentTime, foldTime, HitObject.EndTime, 0, 90);
            var headValue = MathExtensions.Map(currentTime, unfoldTime, HitObject.StartTime, 0, 90);

            Body.SetProgressDegree(headValue, tailValue);
        }

        public override void OnKilled()
        {
            base.OnKilled();
            Body?.RecyclePath();
        }
    }
}
