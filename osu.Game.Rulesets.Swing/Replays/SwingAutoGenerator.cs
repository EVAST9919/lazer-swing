using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Swing.Beatmaps;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Swing.Replays
{
    public class SwingAutoGenerator : AutoGenerator
    {
        public new SwingBeatmap Beatmap => (SwingBeatmap)base.Beatmap;

        public SwingAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
            Replay = new Replay();
        }

        protected Replay Replay;
        protected List<ReplayFrame> Frames => Replay.Frames;

        public override Replay Generate()
        {
            bool hitButton = true;

            Frames.Add(new SwingReplayFrame(-100000));
            Frames.Add(new SwingReplayFrame(Beatmap.HitObjects[0].StartTime - 1000));

            for (int i = 0; i < Beatmap.HitObjects.Count; i++)
            {
                SwingHitObject h = Beatmap.HitObjects[i];
                double endTime = h.GetEndTime();

                switch (h)
                {
                    case Tap hit:
                        {
                            SwingAction[] actions;

                            if (hit.Type == HitType.Down)
                            {
                                actions = new[] { hitButton ? SwingAction.DownSwing : SwingAction.DownSwingAdditional };
                            }
                            else
                            {
                                actions = new[] { hitButton ? SwingAction.UpSwing : SwingAction.UpSwingAdditional };
                            }

                            Frames.Add(new SwingReplayFrame(h.StartTime, actions));
                            break;
                        }

                    default:
                        throw new InvalidOperationException("Unknown hit object type.");
                }

                var nextHitObject = GetNextObject(i); // Get the next object that requires pressing the same button

                bool canDelayKeyUp = nextHitObject == null || nextHitObject.StartTime > endTime + KEY_UP_DELAY;
                double calculatedDelay = canDelayKeyUp ? KEY_UP_DELAY : (nextHitObject.StartTime - endTime) * 0.9;
                Frames.Add(new SwingReplayFrame(endTime + calculatedDelay));

                hitButton = !hitButton;
            }

            return Replay;
        }
    }
}
