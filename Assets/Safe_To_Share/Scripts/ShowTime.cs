using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class ShowTime : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        void OnEnable()
        {
            UpdateText(101);
            DateSystem.NewHour += UpdateText;
            DateSystem.NewMinute += UpdateText;
        }

        void OnDisable() => UnSub();
        void OnDestroy() => UnSub();

        void UnSub()
        {
            DateSystem.NewHour -= UpdateText;
            DateSystem.NewMinute -= UpdateText;
        }

        void UpdateText(int throwAway) => text.text = $"{DateSystem.Hour:00}:{DateSystem.Minute:00}";
    }
}