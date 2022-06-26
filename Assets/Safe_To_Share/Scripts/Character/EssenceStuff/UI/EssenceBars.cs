using UnityEngine;

namespace Character.EssenceStuff.UI
{
    public class EssenceBars : MonoBehaviour
    {
        [SerializeField] EssenceSlider masc, femi;

        public void Setup(EssenceSystem essenceSystem)
        {
            masc.Setup(essenceSystem.Masculinity);
            femi.Setup(essenceSystem.Femininity);
        }
    }
}