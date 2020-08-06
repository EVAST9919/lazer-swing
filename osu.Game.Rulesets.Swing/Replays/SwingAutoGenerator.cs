using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Swing.Beatmaps;
using osu.Game.Rulesets.Swing.Objects;

namespace osu.Game.Rulesets.Swing.Replays
{
    public class SwingAutoGenerator : AutoGenerator
    {
        public new SwingBeatmap Beatmap => (SwingBeatmap)base.Beatmap;

        private const double spinner_hit_speed = 50;

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
                    case Hold _:
                        break;

                    case Spinner spinner:
                        {
                            int d = 0;
                            int count = 0;
                            int req = spinner.RequiredHits + 1;
                            double hitRate = Math.Min(spinner_hit_speed, spinner.Duration / req);

                            for (double j = h.StartTime; j < endTime; j += hitRate)
                            {
                                SwingAction action;

                                switch (d)
                                {
                                    default:
                                    case 0:
                                        action = SwingAction.UpSwing;
                                        break;

                                    case 1:
                                        action = SwingAction.DownSwing;
                                        break;

                                    case 2:
                                        action = SwingAction.UpSwingAdditional;
                                        break;

                                    case 3:
                                        action = SwingAction.DownSwingAdditional;
                                        break;
                                }

                                Frames.Add(new SwingReplayFrame(j, action));
                                d = (d + 1) % 4;
                                if (++count == req)
                                    break;
                            }

                            Frames.Add(new SwingReplayFrame(h.StartTime + spinner_hit_speed * req + 10));

                            break;
                        }

                    case Tap _:
                        break;

                    default:
                        throw new InvalidOperationException("Unknown hit object type.");
                }

                hitButton = !hitButton;
            }

            return Replay;
        }
    }
}
