using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Edit.Blueprints;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Swing.Edit
{
    public class SwingBlueprintContainer : ComposeBlueprintContainer
    {
        public SwingBlueprintContainer(SwingHitObjectComposer composer)
            : base(composer)
        {
        }

        protected override SelectionHandler<HitObject> CreateSelectionHandler() => new SwingSelectionHandler();

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
