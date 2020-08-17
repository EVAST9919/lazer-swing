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

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 512);

        [Resolved(canBeNull: true)]
        private SwingRulesetConfigManager config { get; set; }

        private readonly Bindable<PlayfieldOrientation> orientation = new Bindable<PlayfieldOrientation>(PlayfieldOrientation.Taiko);

        private Container<HitExplosion> explosions;

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
                    rings = new Rings
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    HitObjectContainer,
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
            }, true);
        }

        public override void Add(DrawableHitObject h)
        {
            base.Add(h);
            h.OnNewResult += onNewResult;
        }

        private void onNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            switch (judgedObject)
            {
                case DrawableTap tap:
                    if (result.Type != HitResult.Miss)
                        explosions.Add(new HitExplosion(tap));
                    break;

                case DrawableSpinner spinner:
                    if (result.Type != HitResult.Miss)
                        explosions.Add(new HitExplosion(spinner));
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
            private readonly GlowingHalfRing topGlow;
            private readonly GlowingHalfRing bottomGlow;

            public Rings()
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;
                InternalChildren = new Drawable[]
                {
                    topGlow = new GlowingHalfRing(Color4.DeepSkyBlue)
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Alpha = 0
                    },
                    topRing = new HalfRing
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                    bottomGlow = new GlowingHalfRing(Color4.Red)
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre,
                        Rotation = -180,
                        Alpha = 0
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

                foreach (var effectPoint in working.Value.Beatmap.ControlPointInfo.EffectPoints)
                {
                    if (effectPoint.KiaiMode)
                    {
                        var bpmBeforeKiai = working.Value.Beatmap.ControlPointInfo.TimingPointAt(effectPoint.Time - 1).BeatLength;

                        using (topGlow.BeginAbsoluteSequence(effectPoint.Time - bpmBeforeKiai))
                            topGlow.FadeIn(bpmBeforeKiai, Easing.Out);

                        using (bottomGlow.BeginAbsoluteSequence(effectPoint.Time - bpmBeforeKiai))
                            bottomGlow.FadeIn(bpmBeforeKiai, Easing.Out);
                    }
                    else
                    {
                        var bpmBeforeKiaiOff = working.Value.Beatmap.ControlPointInfo.TimingPointAt(effectPoint.Time - 1).BeatLength;

                        using (topGlow.BeginAbsoluteSequence(effectPoint.Time - bpmBeforeKiaiOff))
                            topGlow.FadeOut(bpmBeforeKiaiOff, Easing.Out);

                        using (bottomGlow.BeginAbsoluteSequence(effectPoint.Time - bpmBeforeKiaiOff))
                            bottomGlow.FadeOut(bpmBeforeKiaiOff, Easing.Out);
                    }
                }
            }

            public void PressTopRing() => topRing.FadeColour(Color4.DeepSkyBlue, 50, Easing.Out);

            public void PressBottomRing() => bottomRing.FadeColour(Color4.Red, 50, Easing.Out);

            public void ReleaseTopRing() => topRing.FadeColour(Color4.White, 300, Easing.Out);

            public void ReleaseBottomRing() => bottomRing.FadeColour(Color4.White, 300, Easing.Out);
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
                InternalChild = new HalfRing(glow_radius)
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
