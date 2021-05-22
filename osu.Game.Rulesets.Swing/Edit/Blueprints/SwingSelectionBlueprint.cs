using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public abstract class SwingSelectionBlueprint<T> : HitObjectSelectionBlueprint<T>
        where T : SwingHitObject
    {
        protected override bool AlwaysShowWhenSelected => false;

        protected SwingSelectionBlueprint(T hitObject)
            : base(hitObject)
        {
        }
    }
}
