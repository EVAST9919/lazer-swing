using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Swing.Objects.Drawables
{
    public class DrawableHold : DrawableSwingHitObject<Hold>
    {
        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        private readonly Container<DrawableHoldHead> headContainer;
        private readonly Container<DrawableHoldBody> holdBodyContainer;

        public DrawableHold(Hold h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                holdBodyContainer = new Container<DrawableHoldBody>(),
                headContainer = new Container<DrawableHoldHead>()
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Type.BindTo(HitObject.TypeBindable);
            Type.BindValueChanged(_ => updateType(), true);
        }

        private void updateType()
        {
            Anchor = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
            Origin = Type.Value == HitType.Up ? Anchor.TopCentre : Anchor.BottomCentre;
        }

        protected override void LoadSamples()
        {
            // Making this empty so the object itself will not produce any sounds
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case HoldHead head:
                    return new DrawableHoldHead(head);

                case HoldBody body:
                    return new DrawableHoldBody(body);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableHoldHead head:
                    headContainer.Child = head;
                    break;

                case DrawableHoldBody body:
                    holdBodyContainer.Child = body;
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            headContainer.Clear();
            holdBodyContainer.Clear();
        }

        public override bool OnPressed(SwingAction action) => false;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset < HitObject.Duration)
                return;

            ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }
    }
}
