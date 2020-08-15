﻿using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Swing.Judgements;

namespace osu.Game.Rulesets.Swing.Objects
{
    public class HoldHeadSound : SwingHitObject
    {
        public override Judgement CreateJudgement() => new SwingJudgement();
    }
}