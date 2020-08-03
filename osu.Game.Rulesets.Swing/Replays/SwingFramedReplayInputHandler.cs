using osu.Game.Rulesets.Replays;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.StateChanges;
using osu.Game.Replays;

namespace osu.Game.Rulesets.Swing.Replays
{
    public class SwingFramedReplayInputHandler : FramedReplayInputHandler<SwingReplayFrame>
    {
        public SwingFramedReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        protected override bool IsImportant(SwingReplayFrame frame) => frame.Actions.Any();

        public override void CollectPendingInputs(List<IInput> inputs)
        {
            inputs.Add(new ReplayState<SwingAction> { PressedActions = CurrentFrame?.Actions ?? new List<SwingAction>() });
        }
    }
}
