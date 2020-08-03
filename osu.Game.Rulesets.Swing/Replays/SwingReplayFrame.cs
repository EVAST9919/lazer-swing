using osu.Game.Beatmaps;
using osu.Game.Replays.Legacy;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Types;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Swing.Replays
{
    public class SwingReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        public List<SwingAction> Actions = new List<SwingAction>();

        public SwingReplayFrame()
        {
        }

        public SwingReplayFrame(double time, params SwingAction[] actions)
            : base(time)
        {
            Actions.AddRange(actions);
        }

        public void FromLegacy(LegacyReplayFrame currentFrame, IBeatmap beatmap, ReplayFrame lastFrame = null)
        {
            if (currentFrame.MouseRight1) Actions.Add(SwingAction.UpSwing);
            if (currentFrame.MouseRight2) Actions.Add(SwingAction.UpSwingAdditional);
            if (currentFrame.MouseLeft1) Actions.Add(SwingAction.DownSwing);
            if (currentFrame.MouseLeft2) Actions.Add(SwingAction.DownSwingAdditional);
        }

        public LegacyReplayFrame ToLegacy(IBeatmap beatmap)
        {
            ReplayButtonState state = ReplayButtonState.None;

            if (Actions.Contains(SwingAction.UpSwing)) state |= ReplayButtonState.Right1;
            if (Actions.Contains(SwingAction.UpSwingAdditional)) state |= ReplayButtonState.Right2;
            if (Actions.Contains(SwingAction.DownSwing)) state |= ReplayButtonState.Left1;
            if (Actions.Contains(SwingAction.DownSwingAdditional)) state |= ReplayButtonState.Left2;

            return new LegacyReplayFrame(Time, null, null, state);
        }
    }
}
