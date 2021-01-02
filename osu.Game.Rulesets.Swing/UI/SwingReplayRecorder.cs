using System.Collections.Generic;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Swing.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingReplayRecorder : ReplayRecorder<SwingAction>
    {
        public SwingReplayRecorder(Score score)
            : base(score)
        {
        }

        protected override ReplayFrame HandleFrame(Vector2 mousePosition, List<SwingAction> actions, ReplayFrame previousFrame) =>
            new SwingReplayFrame(Time.Current, actions.ToArray());
    }
}
