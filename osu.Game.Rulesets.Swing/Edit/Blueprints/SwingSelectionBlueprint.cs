using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public abstract class SwingSelectionBlueprint<T> : OverlaySelectionBlueprint
        where T : SwingHitObject
    {
        protected T HitObject => (T)DrawableObject.HitObject;

        protected override bool AlwaysShowWhenSelected => false;

        protected SwingSelectionBlueprint(DrawableHitObject drawableObject)
            : base(drawableObject)
        {
        }
    }
}
