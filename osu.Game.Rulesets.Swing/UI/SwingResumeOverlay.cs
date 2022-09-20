using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Swing.UI
{
    public class SwingResumeOverlay : ResumeOverlay, IKeyBindingHandler<SwingAction>
    {
        protected override LocalisableString Message => "Press hit key to resume";

        public bool OnPressed(KeyBindingPressEvent<SwingAction> e)
        {
            if (State.Value == Visibility.Hidden)
                return false;

            Resume();
            return true;
        }

        public void OnReleased(KeyBindingReleaseEvent<SwingAction> e)
        {
        }
    }
}
