using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.UI;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osuTK;
using osuTK.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableSpinner : DrawableSwingHitObject<Spinner>
    {
        private readonly Container<DrawableSpinnerTick> ticks;
        private readonly Ring border;
        private readonly Circle filler;

        public DrawableSpinner(Spinner h)
            : base(h)
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AddRangeInternal(new Drawable[]
            {
                filler = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,
                    Size = new Vector2(200),
                    Scale = Vector2.Zero,
                    Colour = Color4.White,
                    Alpha = 0.5f
                },
                border = new Ring
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = Vector2.Zero
                }
            });

            AddInternal(ticks = new Container<DrawableSpinnerTick>());
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableSpinnerTick tick:
                    ticks.Add(tick);
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            ticks.Clear();
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case SpinnerTick tick:
                    return new DrawableSpinnerTick(tick);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            border.ResizeTo(SwingHitObject.DEFAULT_SIZE).Then().ResizeTo(200, HitObject.TimePreempt, Easing.OutQuint);
        }

        private Spinner spinnerObject => (Spinner)HitObject;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (userTriggered)
            {
                DrawableSpinnerTick nextTick = null;

                foreach (var t in ticks)
                {
                    if (!t.IsHit)
                    {
                        nextTick = t;
                        break;
                    }
                }

                nextTick?.TriggerResult(HitResult.Great);

                var numHits = ticks.Count(r => r.IsHit);

                var completion = (float)numHits / spinnerObject.RequiredHits;

                filler.ScaleTo(completion, 260, Easing.OutQuint);

                if (numHits == spinnerObject.RequiredHits)
                    ApplyResult(r => r.Type = HitResult.Great);
            }
            else
            {
                if (timeOffset < 0)
                    return;

                int numHits = 0;

                foreach (var tick in ticks)
                {
                    if (tick.IsHit)
                    {
                        numHits++;
                        continue;
                    }

                    tick.TriggerResult(HitResult.Miss);
                }

                var hitResult = numHits > spinnerObject.RequiredHits / 2 ? HitResult.Good : HitResult.Miss;

                ApplyResult(r => r.Type = hitResult);
            }
        }

        private bool? lastWasUp;

        public override bool OnPressed(SwingAction action)
        {
            // Don't handle keys before the swell starts
            if (Time.Current < HitObject.StartTime)
                return false;

            var isUp = action == SwingAction.UpSwing || action == SwingAction.UpSwingAdditional;

            // Ensure alternating up and down hits
            if (lastWasUp == isUp)
                return false;

            lastWasUp = isUp;

            UpdateResult(true);

            return true;
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            const double transition_duration = 300;

            switch (state)
            {
                case ArmedState.Miss:
                    using (BeginDelayedSequence(spinnerObject.Duration, true))
                    {
                        this.FadeColour(Color4.Red, transition_duration, Easing.OutQuint);
                        this.FadeOut(transition_duration);
                    }

                    break;

                case ArmedState.Hit:
                    using (BeginDelayedSequence(spinnerObject.Duration, true))
                    {
                        this.FadeOut(transition_duration, Easing.OutQuint);
                        this.ScaleTo(0, transition_duration, Easing.OutQuint);
                    }

                    break;
            }
        }
    }
}
