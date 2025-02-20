using System.Collections.Generic;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Swing.Edit.Blueprints;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Swing.Edit
{
    public partial class SwingBlueprintContainer : ComposeBlueprintContainer
    {
        public SwingBlueprintContainer(SwingHitObjectComposer composer)
            : base(composer)
        {
        }

        protected override SelectionHandler<HitObject> CreateSelectionHandler() => new SwingSelectionHandler();

        protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<HitObject> blueprint, Vector2[] originalSnapPositions)> blueprints)
        {
            return true;
        }

        public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject)
        {
            switch (hitObject)
            {
                case Spinner s:
                    return new SpinnerSelectionBlueprint(s);

                case Tap tap:
                    return new TapSelectionBlueprint(tap);

                case Hold hold:
                    return new HoldSelectionBlueprint(hold);
            }

            return base.CreateHitObjectBlueprintFor(hitObject);
        }
    }
}
