using Battle.CombatantStuff;
using Character;

namespace Battle
{
    public class CombatCharacter
    {
        public CombatCharacter(Combatant combatant, BaseCharacter character, bool ally, int optionalStartThreat = 0)
        {
            Combatant = combatant;
            Character = character;
            Ally = ally;
            Threat = optionalStartThreat;
        }

        public int SpeedAccumulated { get; private set; }
        public bool Ally { get; }
        public int Threat { get; private set; }
        public BaseCharacter Character { get; }

        public Combatant Combatant { get; }
        public void IncreaseThreat(int by) => Threat += by;
        public void DecreaseThreat(int by) => Threat -= by;

        public CombatCharacter MyTurn()
        {
            SpeedAccumulated = 0;
            return this;
        }

        public void NewTurn() => SpeedAccumulated += Character.Stats.Agility.Value;
    }
}