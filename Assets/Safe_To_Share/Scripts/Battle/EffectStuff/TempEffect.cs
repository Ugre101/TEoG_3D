using System;
using Character;
using Character.StatsStuff.Mods;

namespace Battle.EffectStuff
{
    [Serializable]
    public abstract class TempEffect : Effect
    {
        public int tempHourOrTurnsDuration;

        public TempIntMod TempIntMod(BaseCharacter user, string from, bool minus = false) =>
            new(tempHourOrTurnsDuration, minus ? -IntValue(user) : IntValue(user), from, modType);
    }
}