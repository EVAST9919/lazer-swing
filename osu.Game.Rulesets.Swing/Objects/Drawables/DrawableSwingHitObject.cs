using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Swing;
using osu.Game.Rulesets.Swing.Objects;
using osu.Game.Rulesets.Swing.UI;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableSwingHitObject : DrawableHitObject<SwingHitObject>
    {
        protected readonly Container Content;

        protected DrawableSwingHitObject(SwingHitObject hitObject)
            : base(hitObject)
        {
            AutoSizeAxes = Axes.Both;
            AddInternal(new Container
            {
                AutoSizeAxes = Axes.X,
                Height = SwingPlayfield.FULL_SIZE.Y / 2,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Y,
                        Width = 2
                    },
                    Content = new Container
                    {
                        Origin = Anchor.Centre
                    }
                }
            });
        }
    }

    public abstract class DrawableSwingHitObject<T> : DrawableSwingHitObject
        where T : SwingHitObject
    {
        protected Vector2 BaseSize;
        protected Drawable MainPiece;

        protected readonly Bindable<HitType> Type = new Bindable<HitType>();

        protected override Color4 GetComboColour(IReadOnlyList<Color4> comboColours) =>
            comboColours[(HitObject.IndexInBeatmap + 1) % comboColours.Count];

        /// <summary>
        /// A list of keys which can result in hits for this HitObject.
        /// </summary>
        public SwingAction[] HitActions { get; private set; }

        /// <summary>
        /// The action that caused this <see cref="DrawableHit"/> to be hit.
        /// </summary>
        public SwingAction? HitAction { get; protected set; }

        protected DrawableSwingHitObject(T hitObject)
            : base(hitObject)
        {
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
            Content.Anchor = Type.Value == HitType.Up ? Anchor.BottomCentre : Anchor.TopCentre;

            RecreatePieces();
        }

        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;

        protected virtual void RecreatePieces()
        {
            Content.Size = new Vector2(SwingHitObject.DEFAULT_SIZE);

            MainPiece?.Expire();
            Content.Add(MainPiece = CreateMainPiece());
        }

        protected abstract Drawable CreateMainPiece();

        protected override void UpdateStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Idle:
                    // Manually set to reduce the number of future alive objects to a bare minimum.
                    LifetimeStart = HitObject.StartTime - HitObject.TimePreempt;
                    break;
            }
        }
    }
}
