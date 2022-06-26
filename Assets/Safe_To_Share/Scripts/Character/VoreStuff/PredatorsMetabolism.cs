using Character.BodyStuff;
using UnityEngine;

namespace Character.VoreStuff
{
    [CreateAssetMenu(fileName = "Create VorePerk Metabolism", menuName = "Character/Vore/VorePerkPredMeta")]
    public class PredatorsMetabolism : VorePerk
    {
        [SerializeField, Range(1f, 100f),] float divValue;

        public override void OnTick(BaseCharacter character)
        {
            float fatRatio = character.Body.GetFatRatio();
            if (fatRatio > 1f)
            {
                float toBurn = character.Body.Fat.BaseValue / divValue;
                character.Body.Fat.BaseValue -= toBurn;
            }
        }
    }
}