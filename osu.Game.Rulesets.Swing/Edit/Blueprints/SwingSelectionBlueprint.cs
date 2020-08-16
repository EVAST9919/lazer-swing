using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Edit.Blueprints
{
    public abstract class SwingSelectionBlueprint<T> : OverlaySelectionBlueprint
        where T : SwingHitObject
    {
        protected new T HitObject => (T)DrawableObject.HitObject;

        protected override bool AlwaysShowWhenSelected => true;

        protected SwingSelectionBlueprint(DrawableHitObject drawableObject)
            : base(drawableObject)
        {
        }
    }
}
