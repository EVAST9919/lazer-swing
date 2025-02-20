using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Input;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public partial class TapPlacementBlueprint : HitObjectPlacementBlueprint
    {
        public new Tap HitObject => (Tap)base.HitObject;

        [Resolved]
        private IBeatSnapProvider beatSnapProvider { get; set; }

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
            piece.UpdateFrom(tap);
        }

        /*public override void UpdateTimeAndPosition(SnapResult result)
        {
            base.UpdateTimeAndPosition(result);

            var localPosition = ToLocalSpace(result.ScreenSpacePosition);
            tap.Type = localPosition.Y > SwingPlayfield.FULL_SIZE.Y / 2 ? HitType.Down : HitType.Up;
            tap.StartTime = beatSnapProvider.SnapTime(EditorClock.CurrentTime + localPosition.X * 1.5f);
        }*/

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    EndPlacement(true);
                    return true;
            }

            return false;
        }
    }
}
