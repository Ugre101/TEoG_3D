using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Options
{
    public sealed class ToggleGoDirectlyToAfterBattle : MonoBehaviour
    {
        public const string SkipVictoryScreen = "SkipVictoryScreen";

        [SerializeField] Toggle toggle;

        // Start is called before the first frame update
        void Start()
        {
            toggle.onValueChanged.AddListener(ChangeValue);
            toggle.isOn = PlayerPrefs.GetInt(SkipVictoryScreen, 0) == 1;
        }

        static void ChangeValue(bool arg0) => PlayerPrefs.SetInt(SkipVictoryScreen, arg0 ? 1 : 0);
    }
}