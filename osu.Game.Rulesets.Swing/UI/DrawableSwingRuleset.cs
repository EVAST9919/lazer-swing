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
using System.Linq;
using osu.Game.Rulesets.Objects;
using osu.Framework.Utils;
using System;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Swing.UI
{
    public class DrawableSwingRuleset : DrawableRuleset<SwingHitObject>
    {
        private SwingPlayfield playfield;

        public DrawableSwingRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            generate(Beatmap).ForEach(playfield.Add);
        }

        protected override PassThroughInputManager CreateInputManager() => new SwingInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => playfield = new SwingPlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new SwingPlayfieldAdjustmentContainer();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new SwingFramedReplayInputHandler(replay);

        protected override ReplayRecorder CreateReplayRecorder(Replay replay) => new SwingReplayRecorder(replay);

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

        private static List<BarLine> generate(IBeatmap beatmap)
        {
            var bars = new List<BarLine>();

            if (beatmap.HitObjects.Count == 0)
                return bars;

            var lastObject = beatmap.HitObjects.Last();
            double lastHitTime = 1 + lastObject.GetEndTime();

            var timingPoints = beatmap.ControlPointInfo.TimingPoints;

            if (timingPoints.Count == 0)
                return bars;

            var timePreempt = (float)BeatmapDifficulty.DifficultyRange(beatmap.BeatmapInfo.BaseDifficulty.ApproachRate, 1800, 1200, 450);

            for (int i = 0; i < timingPoints.Count; i++)
            {
                var currentTimingPoint = timingPoints[i];
                int currentBeat = 0;

                // Stop on the beat before the next timing point, or if there is no next timing point stop slightly past the last object
                double endTime = i < timingPoints.Count - 1 ? timingPoints[i + 1].Time - currentTimingPoint.BeatLength : lastHitTime + currentTimingPoint.BeatLength * (int)currentTimingPoint.TimeSignature;

                double barLength = currentTimingPoint.BeatLength * (int)currentTimingPoint.TimeSignature;

                for (double t = currentTimingPoint.Time; Precision.DefinitelyBigger(endTime, t); t += barLength, currentBeat++)
                {
                    var roundedTime = Math.Round(t, MidpointRounding.AwayFromZero);

                    // in the case of some bar lengths, rounding errors can cause t to be slightly less than
                    // the expected whole number value due to floating point inaccuracies.
                    // if this is the case, apply rounding.
                    if (Precision.AlmostEquals(t, roundedTime))
                    {
                        t = roundedTime;
                    }

                    bool major = currentBeat % (int)currentTimingPoint.TimeSignature == 0;

                    bars.AddRange(new[]
                    {
                        new BarLine
                        {
                            StartTime = t,
                            Major = major,
                            TimePreempt = timePreempt
                        }
                    });
                }
            }

            return bars;
        }
    }
}
