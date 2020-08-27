using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Game.Graphics;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Swing.Extensions;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class PathSliderBody : CompositeDrawable, IHasAccentColour
    {
        private const float shadow_radius = 7;
        private static readonly float radius = SwingHitObject.DEFAULT_SIZE / 4 + shadow_radius;
        private static readonly float half_playfiled = SwingPlayfield.FULL_SIZE.Y / 2;

        private Color4 accentColour;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                accentColour = value;
                sliderPath.AccentColour = value;
            }
        }

        private readonly List<Vector2> currentCurve = new List<Vector2>();

        private readonly Container pathContainer;
        private readonly SliderPath verticesController;

        private DrawableSliderPath sliderPath;
        private readonly Vector2 size;

        public PathSliderBody()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            InternalChild = pathContainer = new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Size = size = new Vector2((half_playfiled + radius) * 2, half_playfiled + radius * 2),
                Y = -radius
            };

            verticesController = new SliderPath(PathType.PerfectCurve, new[]
            {
                new Vector2(half_playfiled * 2, 0),
                new Vector2(half_playfiled, half_playfiled),
                new Vector2(0, 0),
            });

            RecyclePath();
        }

        private double lastStartDegree = -1;
        private double lastEndDegree = -1;

        public void SetProgressDegree(double headDegree, double tailDegree)
        {
            if (headDegree == lastStartDegree && tailDegree == lastEndDegree)
                return;

            var start = headDegree / 180;
            var end = tailDegree / 180;

            verticesController.GetPathToProgress(currentCurve, end, start);

            if (sliderPath != null)
            {
                sliderPath.Vertices = currentCurve;
                sliderPath.OriginPosition = sliderPath.PositionInBoundingBox(Vector2.Zero) - new Vector2(radius);
            }

            lastStartDegree = headDegree;
            lastEndDegree = tailDegree;
        }

        public void RecyclePath()
        {
            pathContainer.Child = sliderPath = new DrawableSliderPath().With(p =>
            {
                p.AccentColour = sliderPath?.AccentColour ?? accentColour;
                p.Vertices = sliderPath?.Vertices ?? Array.Empty<Vector2>();
                p.PathRadius = sliderPath?.PathRadius ?? radius;
            });

            sliderPath.AutoSizeAxes = Axes.None;
            sliderPath.Size = size;
        }

        private class DrawableSliderPath : SmoothPath
        {
            private Color4 accentColour = Color4.White;

            public Color4 AccentColour
            {
                get => accentColour;
                set
                {
                    if (accentColour == value)
                        return;

                    accentColour = value;

                    InvalidateTexture();
                }
            }

            private static readonly float shadow_portion = shadow_radius / radius;
            private static readonly float border_portion = 0.15f + shadow_portion;
            private static readonly float inner_shadow_portion = border_portion + shadow_portion;

            private const float opacity_at_centre = 0.4f;

            protected override Color4 ColourAt(float position)
            {
                if (position <= shadow_portion)
                    return new Color4(0, 0, 0, 0 - (-opacity_at_centre) * position / shadow_portion);

                //return new Color4(AccentColour.R, AccentColour.G, AccentColour.B, MathExtensions.Map(position, 0, shadow_portion, 0, opacity_at_centre)); - glow

                if (position <= border_portion)
                    return Color4.White;

                if (position <= inner_shadow_portion)
                    return AccentColour.Darken(MathExtensions.Map(position, border_portion, inner_shadow_portion, opacity_at_centre, 0));

                return AccentColour;
            }
        }
    }
}
