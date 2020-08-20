﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableSpinner : DrawableSwingHitObject<Spinner>
    {
        private readonly double glowDuration;

        private readonly Container<DrawableSpinnerTick> ticks;
        private readonly CircularProgress filler;
        private readonly FoldableHalfRing ring;
        private readonly Glow glow;

        public DrawableSpinner(Spinner h)
            : base(h)
        {
            glowDuration = Math.Min(50, h.Duration / 10);

            Size = new Vector2(100, 200);
            Anchor = Anchor.Centre;
            Origin = Anchor.CentreLeft;
            AddInternal(new Container
            {
                Size = new Vector2(200),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    filler = new CircularProgress
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        InnerRadius = 0.75f,
                        Colour = Color4.BlueViolet,
                        Alpha = 0.6f
                    },
                    ring = new FoldableHalfRing(RingState.Closed)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Rotation = 90
                    },
                    glow = new Glow
                    {
                        Rotation = 90,
                        Alpha = 0
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

        private float completion;

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            ring.Open(HitObject.TimePreempt);

            using (BeginDelayedSequence(HitObject.TimePreempt, true))
            {
                glow.FadeIn(glowDuration);

                if (Auto)
                {
                    completion = 0.5f;
                    filler.FillTo(completion, HitObject.Duration * 0.9);
                }
            }
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (Auto)
            {
                if (Time.Current > HitObject.StartTime + HitObject.Duration * 0.9f)
                {
                    foreach (var tick in ticks)
                        tick.TriggerResult(HitResult.Great);

                    ApplyResult(r => r.Type = r.Judgement.MaxResult);
                }

                return;
            }

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

                completion = (float)numHits / HitObject.RequiredHits / 2;

                filler.FillTo(completion, 250, Easing.OutQuint);

                if (numHits == HitObject.RequiredHits)
                    ApplyResult(r => r.Type = r.Judgement.MaxResult);
            }
            else
            {
                if (timeOffset < 0)
                {
                    completion = Auto ? 0.5f : 0;
                    return;
                }

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

                ApplyResult(r => r.Type = numHits > HitObject.RequiredHits / 2 ? HitResult.Good : HitResult.Miss);
            }
        }

        private bool? lastWasUp;

        public override bool OnPressed(SwingAction action)
        {
            if (Time.Current > HitObject.EndTime)
                return false;

            if (Time.Current < HitObject.StartTime)
                return false;

            var isUp = action == SwingAction.UpSwing || action == SwingAction.UpSwingAdditional;

            // Ensure alternating up and down hits
            if (lastWasUp == isUp)
                return true;

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
                    using (BeginDelayedSequence(HitObject.Duration, true))
                    {
                        ring.CloseBack(transition_duration);
                        filler.FillTo(completion).Then().FillTo(0, transition_duration, Easing.Out);
                        glow.FadeOut(glowDuration, Easing.OutQuint);
                        this.FadeColour(Color4.Red, transition_duration, Easing.OutQuint);
                        this.Delay(transition_duration).FadeOut().Expire(true);
                    }

                    break;

                case ArmedState.Hit:
                    using (BeginDelayedSequence(HitObject.Duration, true))
                    {
                        ring.Close(transition_duration);
                        glow.FadeOut(glowDuration, Easing.OutQuint);
                        filler.FlashColour(Color4.White, transition_duration, Easing.Out);
                        filler.FillTo(completion).Then().FillTo(0, transition_duration, Easing.Out);
                        filler.RotateTo(180, transition_duration, Easing.Out);
                        this.Delay(transition_duration).FadeOut().Expire(true);
                    }

                    break;
            }
        }

        private class Glow : CompositeDrawable
        {
            private const float glow_size = 4;

            public Glow()
            {
                Origin = Anchor.BottomCentre;
                Anchor = Anchor.Centre;
                Size = new Vector2(200 + (glow_size + 5) * 2, 100 + glow_size + 5);
                InternalChild = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Child = new BasicHalfRing
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Size = new Vector2(200, 100),
                    }
                }.WithEffect(new GlowEffect
                {
                    BlurSigma = new Vector2(glow_size),
                    Strength = 20,
                    Colour = Color4.BlueViolet
                });
            }
        }
    }
}
