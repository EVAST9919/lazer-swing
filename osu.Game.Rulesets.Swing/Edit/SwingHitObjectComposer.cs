using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Swing.Objects;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Edit
{
    public class SwingHitObjectComposer : HitObjectComposer<SwingHitObject>
    {
        public SwingHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => new HitObjectCompositionTool[]
        {
            //new TickCompositionTool(),
            //new AngeledProjectileCompositionTool(),
            //new CircularExplosionCompositionTool(),
            //new ShapedExplosionCompositionTool(),
            //new SpinnerCompositionTool()
        };

        //protected override ComposeBlueprintContainer CreateBlueprintContainer(IEnumerable<DrawableHitObject> hitObjects)
        //    => new TouhosuBlueprintContainer(hitObjects);
    }
}
