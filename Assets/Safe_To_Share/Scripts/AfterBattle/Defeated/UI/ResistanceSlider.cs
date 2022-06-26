using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.UI
{
    public class ResistanceSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI text;

        void Start() => DefeatShared.ResistanceChange += Change;

        void OnDestroy() => DefeatShared.ResistanceChange -= Change;

        void Change(int obj)
        {
            slider.value = obj;
            text.text = obj.ToString();
        }
    }
}