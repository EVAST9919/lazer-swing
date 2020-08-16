using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Swing.Objects;
using osuTK.Input;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public class TapPlacementBlueprint : PlacementBlueprint
    {
        public new Tap HitObject => (Tap)base.HitObject;

        private readonly TapPiece piece;
        private static Tap tap;

        public TapPlacementBlueprint()
            : base(tap = new Tap())
        {
            InternalChild = piece = new TapPiece();
        }

        protected override void Update()
        {
            base.Update();
            piece.UpdateFrom(HitObject);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    tap.Type = HitType.Up;
                    EndPlacement(true);
                    return true;

                case MouseButton.Right:
                    tap.Type = HitType.Down;
                    EndPlacement(true);
                    return true;
            }

            return false;
        }
    }
}
