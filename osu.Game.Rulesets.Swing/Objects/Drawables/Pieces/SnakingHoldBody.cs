﻿using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Swing.Skinning;

namespace osu.Game.Rulesets.Swing.Objects.Drawables.Pieces
{
    public class SnakingHoldBody : SwingSkinnableDrawable
    {
        private const float size = 281;

        protected override string TextureName => "hold-body";

        protected override Sprite SpriteToSkin => sprite;

        private readonly Sprite sprite;
        private readonly Container headContainer;

        public SnakingHoldBody()
        {
            Anchor = Anchor.TopCentre;
            Size = new Vector2(size);
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Name = "Tail",
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.BottomRight,
                    AutoSizeAxes = Axes.Both,
                    Child = new HoldBodyEnd
                    {
                        Anchor = Anchor.BottomCentre,
                        Rotation = -90
                    }
                },
                headContainer = new Container
                {
                    Name = "Head",
                    RelativeSizeAxes = Axes.X,
                    Child = new Container
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        AutoSizeAxes = Axes.Both,
                        Child = new HoldBodyEnd
                        {
                            Anchor = Anchor.TopCentre,
                            Rotation = 90,
                        }
                    }
                },
                new Container
                {
                    Name = "Body",
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    Child = sprite = new Sprite
                    {
                        Anchor = Anchor.TopLeft,
                        Origin = Anchor.BottomLeft,
                        RelativeSizeAxes = Axes.Both
                    }
                },
            };
        }

        private TransformSequence<Drawable> rotateSprite(float newValue, double duration = 0, Easing easing = Easing.None) => ((Drawable)sprite).RotateTo(newValue, duration, easing);
        private TransformSequence<Drawable> rotateHead(float newValue, double duration = 0, Easing easing = Easing.None) => ((Drawable)headContainer).RotateTo(newValue, duration, easing);

        public TransformSequence<Drawable>[] UnfoldToDegree(float newValue, double duration = 0, Easing easing = Easing.None) => new TransformSequence<Drawable>[]
        {
            rotateSprite(newValue, duration, easing),
            rotateHead(newValue, duration, easing)
        };
    }
}
