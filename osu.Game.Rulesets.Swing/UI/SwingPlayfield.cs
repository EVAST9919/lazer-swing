﻿using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 512);

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Ring
                {
                    Size = new Vector2(SwingHitObject.DEFAULT_SIZE),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                HitObjectContainer
            };
        }

        private class Ring : CircularContainer
        {
            public Ring()
            {
                Masking = true;
                BorderColour = Color4.White;
                BorderThickness = 2;
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    AlwaysPresent = true
                };
            }
        }
    }
}
