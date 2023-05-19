using UnityEngine;

namespace Character.EssenceStuff.UI
{
    public sealed class AfterBattleEssenceSliders : MonoBehaviour
    {
        [SerializeField] AfterBattleEssenceSlider masc, femi;

        public void Setup(BaseCharacter character)
        {
            masc.Setup(character, EssenceType.Masc);
            femi.Setup(character, EssenceType.Femi);
        }
    }
}