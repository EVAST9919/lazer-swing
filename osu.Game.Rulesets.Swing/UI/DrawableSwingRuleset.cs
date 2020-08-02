using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.UI
{
    public class DrawableSwingRuleset : DrawableRuleset<SwingHitObject>
    {
        public DrawableSwingRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override PassThroughInputManager CreateInputManager() => new SwingInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => new SwingPlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new SwingPlayfieldAdjustmentContainer();

        public override DrawableHitObject<SwingHitObject> CreateDrawableRepresentation(SwingHitObject h)
        {
            switch (h)
            {
                case TapHitObject tap:
                    return new DrawableTapHitObject(tap);
            }

            return null;
        }
    }
}
