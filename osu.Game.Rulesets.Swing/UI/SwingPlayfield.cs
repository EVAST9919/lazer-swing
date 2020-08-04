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
                        Origin = Anchor.TopCentre,
                        Height = FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2,
                        Width = 1,
                        EdgeSmoothness = Vector2.One,
                        Colour = ColourInfo.GradientVertical(Color4.Black.Opacity(0), Color4.White)
                    },
                    new Box
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Height = FULL_SIZE.Y / 2 - SwingHitObject.DEFAULT_SIZE / 2,
                        Width = 1,
                        EdgeSmoothness = Vector2.One,
                        Colour = ColourInfo.GradientVertical(Color4.White, Color4.Black.Opacity(0))
                    },
                    new Ring
                    {
                        Size = new Vector2(SwingHitObject.DEFAULT_SIZE),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    new HalfRing
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.BottomCentre,
                        Rotation = -180
                    },
                    new HalfRing
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre
                    },
                    HitObjectContainer
                }
            };

            config?.BindWith(SwingRulesetSetting.PlayfieldOrientation, orientation);
            orientation.BindValueChanged(u =>
            {
                Rotation = u.NewValue == PlayfieldOrientation.Mania ? -90 : 0;
            }, true);
        }

        private class HalfRing : Container
        {
            private static readonly float size = FULL_SIZE.X - SwingHitObject.DEFAULT_SIZE;

            public HalfRing()
            {
                Size = new Vector2(size, size / 2);
                Masking = true;
                Child = new Ring
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(size)
                };
            }
        }
    }
}
