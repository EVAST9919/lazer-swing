using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osuTK.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableTapHitObject : DrawableSwingHitObject<TapHitObject>, IKeyBindingHandler<SwingAction>
    {
        private bool validActionPressed;
        private bool pressHandledThisFrame;
        private readonly double rotationTime;
        private readonly double appearTime;

        private Circle circle;

        public DrawableTapHitObject(TapHitObject h)
            : base(h)
        {
            rotationTime = h.TimePreempt * 2;
            appearTime = HitObject.StartTime - HitObject.TimePreempt;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AccentColour.BindValueChanged(accent =>
            {
                circle.Colour = accent.NewValue;
            }, true);
        }

        protected override Drawable CreateMainPiece() => circle = new Circle
        {
            RelativeSizeAxes = Axes.Both,
            Masking = true
        };

        protected override void Update()
        {
            base.Update();

            // The input manager processes all input prior to us updating, so this is the perfect time
            // for us to remove the extra press blocking, before input is handled in the next frame
            pressHandledThisFrame = false;

            var currentTime = Time.Current;

            if (currentTime < appearTime)
                Rotation = Type.Value == HitType.Up ? -90 : 90;
            else
            {
                var rotationOffset = (currentTime - appearTime) / rotationTime * 180;
                if (Type.Value == HitType.Up)
                {
                    Rotation = (float)(-90 + rotationOffset);
                }
                else
                {
                    Rotation = (float)(90 - rotationOffset);
                }
            }
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            this.FadeInFromZero(100, Easing.OutQuint);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                {
                    ApplyResult(r => r.Type = HitResult.Miss);
                }
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            if (validActionPressed)
                ApplyResult(r => r.Type = result);
        }

        public bool OnPressed(SwingAction action)
        {
            if (pressHandledThisFrame)
                return true;
            if (Judged)
                return false;

            validActionPressed = HitActions.Contains(action);

            // Only count this as handled if the new judgement is a hit
            var result = UpdateResult(true);
            if (IsHit)
                HitAction = action;

            // Regardless of whether we've hit or not, any secondary key presses in the same frame should be discarded
            // E.g. hitting a non-strong centre as a strong should not fall through and perform a hit on the next note
            pressHandledThisFrame = true;
            return result;
        }

        public void OnReleased(SwingAction action)
        {
            if (action == HitAction)
                HitAction = null;
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 100);
                    this.FadeOut(100);
                    break;

                case ArmedState.Hit:
                    this.ScaleTo(1.1f, 200, Easing.OutQuint);
                    this.FadeOut(200);
                    break;
            }
        }
    }
}
