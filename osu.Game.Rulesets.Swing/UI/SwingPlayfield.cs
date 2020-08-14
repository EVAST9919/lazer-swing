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

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 512);

        [Resolved(canBeNull: true)]
        private SwingRulesetConfigManager config { get; set; }

        private readonly Bindable<PlayfieldOrientation> orientation = new Bindable<PlayfieldOrientation>(PlayfieldOrientation.Taiko);

        [BackgroundDependencyLoader]
        private void load()
        {
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
                    new Ring
                    {
                        Size = new Vector2(SwingHitObject.DEFAULT_SIZE),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    HitObjectContainer,
                    new Rings
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                }
            };

            config?.BindWith(SwingRulesetSetting.PlayfieldOrientation, orientation);
            orientation.BindValueChanged(u =>
            {
                Rotation = u.NewValue == PlayfieldOrientation.Mania ? -90 : 0;
            }, true);
        }

        private class Rings : CompositeDrawable, IKeyBindingHandler<SwingAction>
        {
            private readonly HalfRing topRing;
            private readonly HalfRing bottomRing;

            public Rings()
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;
                InternalChildren = new Drawable[]
                {
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

            public bool OnPressed(SwingAction action)
            {
                switch (action)
                {
                    case SwingAction.UpSwing:
                    case SwingAction.UpSwingAdditional:
                        topRing.FadeColour(Color4.DeepSkyBlue, 100, Easing.Out);
                        break;

                    case SwingAction.DownSwing:
                    case SwingAction.DownSwingAdditional:
                        bottomRing.FadeColour(Color4.Red, 100, Easing.Out);
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
                        topRing.FadeColour(Color4.White, 300, Easing.Out);
                        break;

                    case SwingAction.DownSwing:
                    case SwingAction.DownSwingAdditional:
                        bottomRing.FadeColour(Color4.White, 300, Easing.Out);
                        break;
                }
            }
        }

        public void Add(BarLine bar) => base.Add(new DrawableBarLine(bar));

        private class HalfRing : Container
        {
            private static readonly float size = FULL_SIZE.X - SwingHitObject.DEFAULT_SIZE;

            [BackgroundDependencyLoader]
            private void load(TextureStore textures)
            {
                Size = new Vector2(size, size / 2);
                Child = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = textures.Get("half-ring-gradient")
                };
            }
        }
    }
}
