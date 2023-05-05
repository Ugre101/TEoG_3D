using System.Linq;
using Character.EssenceStuff;
using Character.Organs;
using Character.Organs.OrgansContainers;

namespace Character
{
    public static class BaseCharacterExtensions
    {
        const int MaxLoop = 99;

        public static void GrowOrgans(this BaseCharacter character)
        {
            static bool DidGrowSmallest(Essence essence, BaseOrgansContainer container)
                => !container.HaveAny() ? container.TryGrowNew(essence) : container.TryGrowSmallest(essence);

            Essence masculinity = character.Essence.Masculinity;
            bool didGrowSomething = true;

            BallsContainer balls = character.SexualOrgans.Balls;
            DicksContainer dicks = character.SexualOrgans.Dicks;
            const float dickDiv = 0.7f;
            int breakOut = 0;
            while (masculinity.Amount > character.Essence.StableEssence.Value && didGrowSomething)
            {
                if (MaxLoop < breakOut) break;
                breakOut++;

                float dickSum = dicks.BaseList.Sum(d => d.BaseValue);
                float ballsSum = balls.BaseList.Sum(b => b.BaseValue);
                didGrowSomething = dickSum == 0 || dickSum * dickDiv <= ballsSum
                    ? DidGrowSmallest(masculinity, dicks)
                    : DidGrowSmallest(masculinity, balls);
            }

            Essence femininity = character.Essence.Femininity;
            didGrowSomething = true;

            VaginaContainer vagina = character.SexualOrgans.Vaginas;
            BoobsContainer boobs = character.SexualOrgans.Boobs;
            const float boobsDiv = 0.7f;
            breakOut = 0;
            while (femininity.Amount > character.Essence.StableEssence.Value && didGrowSomething)
            {
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