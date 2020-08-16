using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Edit.Blueprints;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Edit
{
    public class SwingBlueprintContainer : ComposeBlueprintContainer
    {
        public SwingBlueprintContainer(IEnumerable<DrawableHitObject> drawableHitObjects)
            : base(drawableHitObjects)
        {
        }

        //protected override SelectionHandler CreateSelectionHandler() => new TouhosuSelectionHandler();

        public override OverlaySelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableSpinner spinner:
                    return new SpinnerSelectionBlueprint(spinner);

                case DrawableTap tap:
                    return new TapSelectionBlueprint(tap);
            }

            return base.CreateBlueprintFor(hitObject);
        }
    }
}
