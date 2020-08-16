using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHoldTailCircle : DrawableSwingHitObject<HoldTailCircle>
    {
        private readonly double rotationTime;
        private readonly double appearTime;

        private readonly Container contentContainer;
        private readonly CircularContainer circle;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        public bool IsTracking { get; set; }

        public DrawableHoldTailCircle(HoldTailCircle h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                contentContainer = new Container
                {
                    Height = SwingPlayfield.FULL_SIZE.Y / 2,
                    Child = circle = new CircularContainer
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(SwingHitObject.DEFAULT_SIZE),
                        Masking = true,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                            AlwaysPresent = true,
                        }
                    }
                }
            });

            rotationTime = h.TimePreempt * 2;
            appearTime = HitObject.StartTime - HitObject.TimePreempt;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            HitActions =
                HitObject.Type == HitType.Up
                    ? new[] { SwingAction.UpSwing, SwingAction.UpSwingAdditional }
                    : new[] { SwingAction.DownSwing, SwingAction.DownSwingAdditional };

            Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;

            contentContainer.Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            contentContainer.Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;

            circle.Anchor = Type.Value == HitType.Up ? Anchor.BottomCentre : Anchor.TopCentre;
            circle.EdgeEffect = new EdgeEffectParameters
            {
                Hollow = true,
                Radius = 10,
                Type = EdgeEffectType.Glow,
                Colour = Type.Value == HitType.Up ? Color4.DeepSkyBlue : Color4.Red
            };
        }

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current;
            var rotationOffset = (currentTime - appearTime) / rotationTime * 180;

            if (currentTime < appearTime)
            {
                contentContainer.Rotation = Type.Value == HitType.Up ? -90 : 90;
                return;
            }

            if (currentTime < HitObject.StartTime)
            {
                contentContainer.Rotation = Type.Value == HitType.Up ? (float)(-90 + rotationOffset) : (float)(90 - rotationOffset);
            }
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
                ApplyResult(r => r.Type = IsTracking || Auto ? r.Judgement.MaxResult : HitResult.Miss);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimePreempt / 3, Easing.Out);
        }

        public override bool OnPressed(SwingAction action) => false;
    }
}
