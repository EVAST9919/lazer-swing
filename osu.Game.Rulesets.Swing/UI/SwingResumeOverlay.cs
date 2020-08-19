using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingResumeOverlay : ResumeOverlay, IKeyBindingHandler<SwingAction>
    {
        protected override string Message => "Press hit key to resume";

        public bool OnPressed(SwingAction action)
        {
            if (State.Value == Visibility.Hidden)
                return false;

            Resume();
            return true;
        }

        public void OnReleased(SwingAction action)
        {
        }
    }
}
