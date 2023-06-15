using System.Linq;
using Character.EssenceStuff;
using Character.Organs.OrgansContainers;

namespace Character {
    public static class BaseCharacterExtensions {
        const int MaxLoop = 99;

        public static void GrowOrgans(this BaseCharacter character) {
            static bool DidGrowSmallest(Essence essence, BaseOrgansContainer container) =>
                !container.HaveAny() ? container.TryGrowNew(essence) : container.TryGrowSmallest(essence);

            var masculinity = character.Essence.Masculinity;
            var didGrowSomething = true;

            var balls = character.SexualOrgans.Balls;
            var dicks = character.SexualOrgans.Dicks;
            const float dickDiv = 0.7f;
            var breakOut = 0;
            while (masculinity.Amount > character.Essence.StableEssence.Value && didGrowSomething) {
                if (MaxLoop < breakOut) break;
                breakOut++;

                float dickSum = dicks.BaseList.Sum(d => d.BaseValue);
                float ballsSum = balls.BaseList.Sum(b => b.BaseValue);
                didGrowSomething = dickSum == 0 || dickSum * dickDiv <= ballsSum
                    ? DidGrowSmallest(masculinity, dicks)
                    : DidGrowSmallest(masculinity, balls);
            }

            var femininity = character.Essence.Femininity;
            didGrowSomething = true;

            var vagina = character.SexualOrgans.Vaginas;
            var boobs = character.SexualOrgans.Boobs;
            const float boobsDiv = 0.7f;
            breakOut = 0;
            while (femininity.Amount > character.Essence.StableEssence.Value && didGrowSomething) {
                if (MaxLoop < breakOut) break;
                breakOut++;
                float vagSum = vagina.BaseList.Sum(v => v.BaseValue);
                float boobsSum = boobs.BaseList.Sum(b => b.BaseValue);
                didGrowSomething = boobsSum == 0 || boobsSum * boobsDiv <= vagSum
                    ? DidGrowSmallest(femininity, boobs)
                    : DidGrowSmallest(femininity, vagina);
            }
        }
    }
}