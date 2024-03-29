﻿using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using System;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Swing.Difficulty;
using osu.Game.Rulesets.Swing.UI;
using osu.Game.Rulesets.Swing.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Swing.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Swing.Mods;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Swing.Replays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Swing.Edit;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Scoring;
using System.Linq;
using osu.Game.Rulesets.Swing.Objects;
using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Swing
{
    public partial class SwingRuleset : Ruleset
    {
        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableSwingRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new SwingBeatmapConverter(beatmap, this);

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new SwingRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new SwingSettingsSubsection(this);

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Z, SwingAction.UpSwing),
            new KeyBinding(InputKey.X, SwingAction.UpSwingAdditional),
            new KeyBinding(InputKey.N, SwingAction.DownSwing),
            new KeyBinding(InputKey.M, SwingAction.DownSwingAdditional)
        };

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new SwingModEasy(),
                        new SwingModNoFail(),
                        new MultiMod(new SwingModHalfTime(), new SwingModDaycore())
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new SwingModSuddenDeath(),
                        new MultiMod(new SwingModDoubleTime(), new SwingModNightcore())
                    };

                case ModType.Automation:
                    return new Mod[]
                    {
                        new SwingModAutoplay()
                    };

                case ModType.Fun:
                    return new Mod[]
                    {
                        new MultiMod(new ModWindUp(), new ModWindDown())
                    };

                case ModType.Conversion:
                    return new Mod[]
                    {
                        new SwingModDifficultyAdjust(),
                        new SwingModNoSliders(),
                        new SwingModAlternate()
                    };

                default:
                    return Array.Empty<Mod>();
            }
        }

        public override string Description => "Swing";

        public override string ShortName => "Swing";

        public override string PlayingVerb => "Rides on a swing";

        public override Drawable CreateIcon() => new SwingIcon(this);

        public override IConvertibleReplayFrame CreateConvertibleReplayFrame() => new SwingReplayFrame();

        public override HitObjectComposer CreateHitObjectComposer() => new SwingHitObjectComposer(this);

        protected override IEnumerable<HitResult> GetValidHitResults() => new[]
        {
            HitResult.Great,
            HitResult.Good
        };

        public override StatisticItem[] CreateStatisticsForScore(ScoreInfo score, IBeatmap playableBeatmap) => new[]
        {
            new StatisticItem("Timing Distribution", () => new HitEventTimingDistributionGraph(score.HitEvents.Where(e => e.HitObject is Tap || e.HitObject is HoldHead).ToList())
            {
                RelativeSizeAxes = Axes.X,
                Height = 250
            }, true)
        };

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new SwingDifficultyCalculator(RulesetInfo, beatmap);

        private partial class SwingIcon : Sprite
        {
            private readonly SwingRuleset ruleset;

            public SwingIcon(SwingRuleset ruleset)
            {
                this.ruleset = ruleset;
            }

            [BackgroundDependencyLoader]
            private void load(GameHost host)
            {
                Texture = new TextureStore(host.Renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/logo");
            }
        }
    }
}
