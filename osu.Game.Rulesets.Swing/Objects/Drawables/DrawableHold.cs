using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.Objects.Drawables.Pieces;
using osuTK;
using osuTK.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public partial class DrawableHold : DrawableSwingHitObject<Hold>
    {
        protected override bool RequiresTimedUpdate => true;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public readonly PathSliderBody Body;
        public readonly HoldBall Ball;

        private DrawableHoldTail tail => tailContainer.Child;
        private DrawableHoldHead head => headContainer.Child;

        private readonly Container<DrawableHoldHead> headContainer;
        private readonly Container<DrawableHoldTail> tailContainer;

        private double unfoldTime;
        private double foldTime;

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
            Body.AccentColour = Type.Value == HitType.Up ? Tap.COLOUR_TOP : Tap.COLOUR_BOTTOM;
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

        public override bool OnPressed(KeyBindingPressEvent<SwingAction> e)
        {
            if (HitActions.Contains(e.Action))
                HitAction = e.Action;

            return false;
        }

        public override void OnReleased(KeyBindingReleaseEvent<SwingAction> e)
        {
            if (e.Action == HitAction)
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

            var tracking = (HitAction != null || Auto) && !IsHit && currentTime > HitObject.StartTime;

            if (currentTime > HitObject.StartTime)
                Ball.Tracking.Value = tracking;

            if (currentTime < HitObject.EndTime)
                return;

            switch (head.Result.Type)
            {
                case HitResult.Great:
                    tail.TriggerResult(tracking ? HitResult.Great : HitResult.Good);
                    break;

                case HitResult.Good:
                case HitResult.Miss:
                    tail.TriggerResult(tracking ? HitResult.Good : HitResult.Miss);
                    break;
            }

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

            unfoldTime = HitObject.StartTime - HitObject.TimePreempt;
            foldTime = unfoldTime + HitObject.Duration;

            Body.HeadAngle = Interpolation.ValueAt(MathHelper.Clamp(currentTime, unfoldTime, HitObject.StartTime), 0f, 90f, unfoldTime, HitObject.StartTime);
            Body.TailAngle = Interpolation.ValueAt(MathHelper.Clamp(currentTime, foldTime, HitObject.EndTime), 0f, 90f, foldTime, HitObject.EndTime);
        }
    }
}
