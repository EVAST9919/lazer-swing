using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Swing.Edit.Blueprints;
using osu.Game.Rulesets.Swing.Objects;
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

        public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject)
        {
            switch (hitObject)
            {
                case Spinner s:
                    return new SpinnerSelectionBlueprint(s);

                case Tap tap:
                    return new TapSelectionBlueprint(tap);
            }

            return base.CreateHitObjectBlueprintFor(hitObject);
        }
    }
}
