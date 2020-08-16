using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Swing.Edit.Blueprints;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Edit
{
    public class SpinnerCompositionTool : HitObjectCompositionTool
    {
        public SpinnerCompositionTool()
            : base(nameof(Spinner))
        {
        }

        public override PlacementBlueprint CreatePlacementBlueprint() => new SpinnerPlacementBlueprint();
    }
}
