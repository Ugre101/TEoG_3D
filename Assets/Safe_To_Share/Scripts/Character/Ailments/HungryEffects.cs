using Character.BodyStuff;

namespace Character.Ailments
{
    public static class HungryEffects
    {
        static readonly Hungry Hungry = new();
        static readonly Starving Starving = new();

        public static bool CheckHungry(this BaseCharacter character)
        {
            if (character.Body.GetFatRatio() >= 0.3f && character.Body.GetFatRatio() < 0.5f)
                return Hungry.Gain(character) | Starving.Cure(character);
            if (character.Body.GetFatRatio() < 0.3f)
                return Hungry.Cure(character) | Starving.Gain(character);
            return Hungry.Cure(character) | Starving.Cure(character);
        }
    }
} 