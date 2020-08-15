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
        private readonly Circle filler;
        private readonly Box line;
        private readonly FoldableHalfRing ring1;
        private readonly FoldableHalfRing ring2;

        public DrawableSpinner(Spinner h)
            : base(h)
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AddRangeInternal(new Drawable[]
            {
                line = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Height = 0.5f,
                    EdgeSmoothness = Vector2.One
                },
                filler = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,
                    Size = new Vector2(200),
                    Scale = Vector2.Zero,
                    Colour = Color4.BlueViolet,
                    Alpha = 0.6f
                },
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(200),
                    Children = new Drawable[]
                    {
                        ring1 = new FoldableHalfRing(RingState.Closed)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        ring2 = new FoldableHalfRing(RingState.Closed)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Rotation = 180
                        }
                    }
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

            line.ResizeWidthTo(198, HitObject.TimePreempt / 2, Easing.Out).Then().RotateTo(180, HitObject.TimePreempt / 2, Easing.Out);

            using (BeginDelayedSequence(HitObject.TimePreempt / 2, true))
            {
                ring1.Open(HitObject.TimePreempt / 2);
                ring2.Open(HitObject.TimePreempt / 2);
            };
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

                ApplyResult(r => r.Type = numHits > spinnerObject.RequiredHits / 2 ? HitResult.Good : HitResult.Miss);
            }
        }

        private bool? lastWasUp;

        public override bool OnPressed(SwingAction action)
        {
            if (Time.Current > HitObject.StartTime + spinnerObject.Duration)
                return false;

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
                        line.RotateTo(0, transition_duration, Easing.Out);
                        ring1.CloseBack(transition_duration);
                        ring2.CloseBack(transition_duration);
                        this.FadeColour(Color4.Red, transition_duration, Easing.OutQuint);
                        this.FadeOut(transition_duration, Easing.Out);
                    }

                    break;

                case ArmedState.Hit:
                    using (BeginDelayedSequence(spinnerObject.Duration, true))
                    {
                        line.RotateTo(360, transition_duration, Easing.Out).Then().ResizeWidthTo(0, transition_duration, Easing.Out);
                        ring1.Close(transition_duration);
                        ring2.Close(transition_duration);
                        this.Delay(transition_duration).FadeOut(transition_duration, Easing.Out);
                    }

                    break;
            }
        }
    }
}
