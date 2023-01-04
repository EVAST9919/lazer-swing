using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.Objects.Drawables;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Swing.Replays;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects;
using System;
using osu.Game.Screens.Play;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Swing.UI
{
    public partial class DrawableSwingRuleset : DrawableRuleset<SwingHitObject>
    {
        private SwingPlayfield playfield;

        public DrawableSwingRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            float timePreempt = (float)IBeatmapDifficultyInfo.DifficultyRange(Beatmap.BeatmapInfo.Difficulty.ApproachRate, 1800, 1200, 450);

            foreach (var b in new BarLineGenerator<BarLine>(Beatmap).BarLines)
            {
                b.TimePreempt = timePreempt;
                playfield.Add(b);
            }
        }

        protected override PassThroughInputManager CreateInputManager() => new SwingInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => playfield = new SwingPlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new SwingPlayfieldAdjustmentContainer();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new SwingFramedReplayInputHandler(replay);

        protected override ReplayRecorder CreateReplayRecorder(Score score) => new SwingReplayRecorder(score);

        protected override ResumeOverlay CreateResumeOverlay() => new SwingResumeOverlay();

        public override void RequestResume(Action continueResume)
        {
            ResumeOverlay.ResumeAction = continueResume;
            ResumeOverlay.Show();
        }

        public override DrawableHitObject<SwingHitObject> CreateDrawableRepresentation(SwingHitObject h)
        {
            switch (h)
            {
                case Hold hold:
                    return new DrawableHold(hold);

                case Spinner spinner:
                    return new DrawableSpinner(spinner);

                case Tap tap:
                    return new DrawableTap(tap);

                case BarLine line:
                    return new DrawableBarLine(line);
            }

            return null;
        }
    }
}
