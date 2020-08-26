using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class PathSliderBody : CompositeDrawable
    {
        private static readonly float radius = SwingHitObject.DEFAULT_SIZE / 4;
        private static readonly float half_playfiled = SwingPlayfield.FULL_SIZE.Y / 2;

        private Color4 colour;

        public new Color4 Colour
        {
            get => colour;
            set
            {
                colour = value;
                if (sliderPath != null)
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
                p.AccentColour = sliderPath?.AccentColour ?? Color4.White;
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

            protected const float BORDER_PORTION = 0.256f;
            protected const float GRADIENT_PORTION = 1 - BORDER_PORTION;

            private const float opacity_at_centre = 0.3f;
            private const float opacity_at_edge = 0.8f;

            protected override Color4 ColourAt(float position)
            {
                if (BORDER_PORTION != 0f && position <= BORDER_PORTION)
                    return Color4.White;

                return AccentColour;

                //position -= BORDER_PORTION;
                //return new Color4(AccentColour.R, AccentColour.G, AccentColour.B, (opacity_at_edge - (opacity_at_edge - opacity_at_centre) * position / GRADIENT_PORTION) * AccentColour.A);
            }
        }
    }
}
