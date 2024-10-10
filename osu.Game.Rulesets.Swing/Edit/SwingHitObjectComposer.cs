using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Edit
{
    public partial class SwingHitObjectComposer : HitObjectComposer<SwingHitObject>
    {
        public SwingHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        protected override IReadOnlyList<CompositionTool> CompositionTools => new CompositionTool[]
        {
            new SpinnerCompositionTool(),
            new TapCompositionTool()
        };

        protected override ComposeBlueprintContainer CreateBlueprintContainer() => new SwingBlueprintContainer(this);
    }
}
