using osu.Framework.Allocation;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Swing.Edit
{
    public class SwingSelectionHandler : SelectionHandler
    {
        [Resolved]
        private IBeatSnapProvider beatSnapProvider { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        public override bool HandleMovement(MoveSelectionEvent moveEvent)
        {
            var selected = SelectedHitObjects.OfType<SwingHitObject>();
            var currentTime = editorClock.CurrentTime;

            foreach (var h in selected)
            {
                // Disallow movement if spinner is selected
                if (h is Spinner)
                    return false;
            }

            var localPosition = ToLocalSpace(moveEvent.ScreenSpacePosition);

            double minTime = double.MaxValue;

            foreach (var h in selected)
            {
                minTime = Math.Min(h.StartTime, minTime);
            }

            foreach (var h in selected)
            {
                var offset = h.StartTime - minTime;
                h.StartTime = beatSnapProvider.SnapTime(currentTime + offset + localPosition.X * 1.5f);
            }

            return true;
        }
    }
}
