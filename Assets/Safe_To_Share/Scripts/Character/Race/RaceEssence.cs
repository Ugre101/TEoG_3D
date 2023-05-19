using Character.Race.Races;

namespace Character.Race
{
    public sealed class RaceEssence
    {
        public RaceEssence(BasicRace race, int amount = 100)
        {
            Race = race;
            Amount = amount;
        }

        public BasicRace Race { get; }

        public int Amount { get; private set; }

        public void SetAmount(int value) => Amount = value;

        public void IncreaseAmount(int value) => Amount += value;

        public bool DecreaseAmount(int value)
        {
            Amount -= value;
            return Amount <= 0;
        }
    }
}