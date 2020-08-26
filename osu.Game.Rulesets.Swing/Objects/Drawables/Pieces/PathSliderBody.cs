using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class PathSliderBody : CompositeDrawable
    {
        private static readonly float radius = SwingHitObject.DEFAULT_SIZE / 4;
        private static readonly float half_playfiled = SwingPlayfield.FULL_SIZE.Y / 2;

        public new Color4 Colour
        {
            get => sliderPath.AccentColour;
            set => sliderPath.AccentColour = value;
        }

        private readonly DrawableSliderPath sliderPath;

        public PathSliderBody()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            InternalChild = new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Size = new Vector2((half_playfiled + radius) * 2, half_playfiled + radius * 2),
                Y = -radius,
                Child = sliderPath = new DrawableSliderPath(radius)
            };
        }

        public void SetProgressDegree(double headDegree, double tailDegree) => sliderPath.SetProgressDegree(headDegree / 180, tailDegree / 180);

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

            private double progress;

            public double Progress
            {
                get => progress;
                set
                {
                    progress = value;
                    setProgress(value);
                }
            }

            private readonly SliderPath path;
            private readonly List<Vector2> newVertices = new List<Vector2>();

            public DrawableSliderPath(float radius)
            {
                PathRadius = radius;
                AutoSizeAxes = Axes.None;
                RelativeSizeAxes = Axes.Both;

                Vector2[] points = new[]
                {
                    new Vector2(half_playfiled * 2, 0),
                    new Vector2(half_playfiled, half_playfiled),
                    new Vector2(0, 0),
                };

                path = new SliderPath(PathType.PerfectCurve, points);
            }

            private void setProgress(double progress)
            {
                path.GetPathToProgress(newVertices, 0, progress);
                Vertices = newVertices;
            }

            public void SetProgressDegree(double start, double end)
            {
                path.GetPathToProgress(newVertices, end, start);
                Vertices = newVertices;
                OriginPosition = PositionInBoundingBox(Vector2.Zero) - new Vector2(radius);
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
