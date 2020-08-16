using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Swing.Beatmaps;

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

        public override Replay Generate() => Replay;
    }
}
