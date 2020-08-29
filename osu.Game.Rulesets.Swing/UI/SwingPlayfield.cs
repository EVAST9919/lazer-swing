using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Configuration;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Colour;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using System;
using osu.Framework.Graphics.Effects;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Framework.Extensions.IEnumerableExtensions;

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 512);

        [Resolved(canBeNull: true)]
        private SwingRulesetConfigManager config { get; set; }

        private readonly Bindable<PlayfieldOrientation> orientation = new Bindable<PlayfieldOrientation>(PlayfieldOrientation.Taiko);

        private Container<HitExplosion> explosions;
        private JudgementContainer<DrawableSwingJudgement> judgementContainer;
        private JudgementContainer<DrawableSwingJudgement> smallJudgementContainer;

        [BackgroundDependencyLoader]
        private void load()
        {
            HitReceptor hitReceptor;
            Rings rings;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                X = -150,
                Children = new Drawable[]
                {
                    rings = new Rings
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    new Box
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.BottomCentre,
                        Y = FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 - 1,
                        Height = FULL_SIZE.Y / 4,
                        Width = 0.5f,
                        EdgeSmoothness = Vector2.One,
                        Colour = ColourInfo.GradientVertical(Color4.Black.Opacity(0), Color4.White)
                    },
                    new Box
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre,
                        Y = - (FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2 - 1),
                        Height = FULL_SIZE.Y / 4,
                        Width = 0.5f,
                        EdgeSmoothness = Vector2.One,
                        Colour = ColourInfo.GradientVertical(Color4.White, Color4.Black.Opacity(0))
                    },
                    explosions = new Container<HitExplosion>
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    new Ring
                    {
                        Size = new Vector2(SwingHitObject.DEFAULT_SIZE),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    HitObjectContainer,
                    judgementContainer = new JudgementContainer<DrawableSwingJudgement>
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        X = 250
                    },
                    smallJudgementContainer = new JudgementContainer<DrawableSwingJudgement>
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        X = -50,
                        Scale = new Vector2(0.5f)
                    },
                    hitReceptor = new HitReceptor()
                }
            };

            hitReceptor.UpKeyPressed += rings.PressTopRing;
            hitReceptor.DownKeyPressed += rings.PressBottomRing;
            hitReceptor.UpKeyReleased += rings.ReleaseTopRing;
            hitReceptor.DownKeyReleased += rings.ReleaseBottomRing;

            config?.BindWith(SwingRulesetSetting.PlayfieldOrientation, orientation);
            orientation.BindValueChanged(u =>
            {
                Rotation = u.NewValue == PlayfieldOrientation.Mania ? -90 : 0;
                judgementContainer.Rotation = u.NewValue == PlayfieldOrientation.Taiko ? 0 : 90;
                smallJudgementContainer.Rotation = u.NewValue == PlayfieldOrientation.Taiko ? 0 : 90;
            }, true);
        }

        public override void Add(DrawableHitObject h)
        {
            base.Add(h);

            switch (h)
            {
                case DrawableHold hold:
                    hold.NestedHitObjects.ForEach(n => n.OnNewResult += onNewResult);
                    return;
            }

            h.OnNewResult += onNewResult;
        }

        private void onNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            switch (judgedObject)
            {
                case DrawableTap _:
                case DrawableHoldHead _:
                case DrawableHoldTail _:
                case DrawableSpinner _:
                    if (result.Type != HitResult.Miss)
                        explosions.Add(new HitExplosion((DrawableSwingHitObject)judgedObject));
                    break;
            }

            if (!DisplayJudgements.Value)
                return;

            switch (judgedObject)
            {
                case DrawableTap _:
                case DrawableHoldHead _:
                case DrawableHoldTail _:
                case DrawableSpinner _:
                    judgementContainer.Add(new DrawableSwingJudgement(result, judgedObject));
                    break;

                case DrawableHoldTick _:
                    smallJudgementContainer.Add(new DrawableSwingJudgement(result, judgedObject));
                    break;
            }
        }

        public void Add(BarLine bar) => base.Add(new DrawableBarLine(bar));

        private class Rings : CompositeDrawable
        {
            [Resolved]
            private Bindable<WorkingBeatmap> working { get; set; }

            private readonly HalfRing topRing;
            private readonly HalfRing bottomRing;
            private readonly GlowContainer glowContainer;

            public Rings()
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;
                InternalChildren = new Drawable[]
                {
                    glowContainer = new GlowContainer(),
                    topRing = new HalfRing
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                    bottomRing = new HalfRing
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre,
                        Rotation = -180
                    }
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                var controlPoints = working.Value.Beatmap.ControlPointInfo;

                foreach (var effectPoint in controlPoints.EffectPoints)
                {
                    if (effectPoint.KiaiMode)
                    {
                        var bpmBeforeKiai = controlPoints.TimingPointAt(effectPoint.Time - 1).BeatLength;

                        using (glowContainer.BeginAbsoluteSequence(effectPoint.Time - bpmBeforeKiai))
                            glowContainer.FadeIn(bpmBeforeKiai, Easing.Out);
                    }
                    else
                    {
                        var bpmBeforeKiaiOff = controlPoints.TimingPointAt(effectPoint.Time - 1).BeatLength;

                        using (glowContainer.BeginAbsoluteSequence(effectPoint.Time - bpmBeforeKiaiOff))
                            glowContainer.FadeOut(bpmBeforeKiaiOff, Easing.Out);
                    }
                }
            }

            public void PressTopRing() => topRing.FadeColour(Color4.DeepSkyBlue, 50, Easing.Out);

            public void PressBottomRing() => bottomRing.FadeColour(Color4.Red, 50, Easing.Out);

            public void ReleaseTopRing() => topRing.FadeColour(Color4.White, 300, Easing.Out);

            public void ReleaseBottomRing() => bottomRing.FadeColour(Color4.White, 300, Easing.Out);

            private class GlowContainer : Container
            {
                public override bool RemoveCompletedTransforms => false;

                public GlowContainer()
                {
                    Anchor = Anchor.Centre;
                    Origin = Anchor.Centre;
                    RelativeSizeAxes = Axes.Y;
                    Alpha = 0;

                    AddRange(new[]
                    {
                        new GlowingHalfRing(Color4.DeepSkyBlue)
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                        },
                        new GlowingHalfRing(Color4.Red)
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.TopCentre,
                            Rotation = -180,
                        }
                    });
                }
            }
        }

        private class HitReceptor : CompositeDrawable, IKeyBindingHandler<SwingAction>
        {
            public Action UpKeyPressed;
            public Action DownKeyPressed;
            public Action UpKeyReleased;
            public Action DownKeyReleased;

            public bool OnPressed(SwingAction action)
            {
                switch (action)
                {
                    case SwingAction.UpSwing:
                    case SwingAction.UpSwingAdditional:
                        UpKeyPressed?.Invoke();
                        break;

                    case SwingAction.DownSwing:
                    case SwingAction.DownSwingAdditional:
                        DownKeyPressed?.Invoke();
                        break;
                }

                return false;
            }

            public void OnReleased(SwingAction action)
            {
                switch (action)
                {
                    case SwingAction.UpSwing:
                    case SwingAction.UpSwingAdditional:
                        UpKeyReleased?.Invoke();
                        break;

                    case SwingAction.DownSwing:
                    case SwingAction.DownSwingAdditional:
                        DownKeyReleased?.Invoke();
                        break;
                }
            }
        }

        private class HalfRing : Container
        {
            private static readonly float size = FULL_SIZE.X - SwingHitObject.DEFAULT_SIZE;
            private readonly float padding;

            public HalfRing(float padding = 0)
            {
                this.padding = padding;
            }

            [BackgroundDependencyLoader]
            private void load(TextureStore textures)
            {
                Size = new Vector2(size, size / 2 + padding);
                Child = new Sprite
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(size, size / 2),
                    Texture = textures.Get("half-ring-gradient")
                };
            }
        }

        private class GlowingHalfRing : CompositeDrawable
        {
            private const float glow_radius = 7;

            public GlowingHalfRing(Color4 colour)
            {
                Anchor = Anchor.TopCentre;
                Origin = Anchor.TopCentre;
                InternalChild = new HalfRing(glow_radius + 5)
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                }.WithEffect(new GlowEffect
                {
                    Colour = colour,
                    Strength = 10,
                    BlurSigma = new Vector2(glow_radius)
                });
            }
        }
    }
}
