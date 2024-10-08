﻿using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Swing.Objects;
using osuTK.Input;
using System;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public class SpinnerPlacementBlueprint : HitObjectPlacementBlueprint
    {
        public new Spinner HitObject => (Spinner)base.HitObject;

        private readonly SpinnerPiece piece;

        public SpinnerPlacementBlueprint()
            : base(new Spinner())
        {
            InternalChild = piece = new SpinnerPiece();
        }

        private bool isPlacingEnd;

        protected override void Update()
        {
            base.Update();

            if (isPlacingEnd)
                HitObject.Duration = Math.Max(0, EditorClock.CurrentTime - HitObject.StartTime);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (isPlacingEnd)
            {
                if (e.Button != MouseButton.Right)
                    return false;

                HitObject.Duration = EditorClock.CurrentTime - HitObject.StartTime;
                EndPlacement(true);
            }
            else
            {
                if (e.Button != MouseButton.Left)
                    return false;

                BeginPlacement(commitStart: true);
                piece.FadeTo(1f, 150, Easing.OutQuint);

                isPlacingEnd = true;
            }

            return true;
        }
    }
}
