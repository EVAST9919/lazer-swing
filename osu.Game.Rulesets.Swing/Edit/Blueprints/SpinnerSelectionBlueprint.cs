using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Swing.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public class SpinnerSelectionBlueprint : SwingSelectionBlueprint<Spinner>
    {
        protected new DrawableSpinner DrawableObject => (DrawableSpinner)base.DrawableObject;

        protected readonly SpinnerPiece Piece;

        public SpinnerSelectionBlueprint(Spinner s)
            : base(s)
        {
            InternalChild = Piece = new SpinnerPiece();
        }

        public override Quad SelectionQuad => DrawableObject.ScreenSpaceDrawQuad;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => DrawableObject.ReceivePositionalInputAt(screenSpacePos);
    }
}
