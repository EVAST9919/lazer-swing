using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Swing.Edit
{
    public partial class SwingHitObjectComposer : HitObjectComposer<SwingHitObject, SwingAction>
    {
        public SwingHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        protected override IReadOnlyList<CompositionTool<SwingAction>> CompositionTools => new CompositionTool<SwingAction>[]
        {
            new SpinnerCompositionTool(),
            new TapCompositionTool()
        };

        protected override ComposeBlueprintContainer CreateBlueprintContainer() => new SwingBlueprintContainer(this);
        public override Bindable<TernaryState> SelectionNewComboState { get; } = new Bindable<TernaryState>();
    }
}
