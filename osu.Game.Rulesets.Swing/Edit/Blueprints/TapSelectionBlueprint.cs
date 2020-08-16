using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public class TapSelectionBlueprint : SwingSelectionBlueprint<Tap>
    {
        protected new DrawableTap DrawableObject => (DrawableTap)base.DrawableObject;

        protected readonly TapPiece Piece;

        public TapSelectionBlueprint(DrawableTap drawable)
            : base(drawable)
        {
            RelativeSizeAxes = Axes.None;
            InternalChild = Piece = new TapPiece();
        }

        protected override void Update()
        {
            base.Update();
            Piece.UpdateFrom(HitObject);
        }

        public override Quad SelectionQuad => Piece.ScreenSpaceDrawQuad;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => Piece.ReceivePositionalInputAt(screenSpacePos);
    }
}
