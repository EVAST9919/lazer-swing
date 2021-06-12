using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Swing.Judgements;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class Tap : SwingHitObject, IHasDisplayColour
    {
        public override Judgement CreateJudgement() => new SwingJudgement();

        public Bindable<Color4> DisplayColour { get; } = new Bindable<Color4>(COLOUR_TOP);

        public static readonly Color4 COLOUR_TOP = Color4.DeepSkyBlue;
        public static readonly Color4 COLOUR_BOTTOM = Color4.Red;

        public Tap()
        {
            TypeBindable.BindValueChanged(_ =>
            {
                DisplayColour.Value = Type == HitType.Up ? COLOUR_TOP : COLOUR_BOTTOM;
            });
        }
    }
}
