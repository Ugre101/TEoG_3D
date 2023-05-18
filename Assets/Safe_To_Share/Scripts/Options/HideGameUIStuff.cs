using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Options
{
    public sealed class HideGameUIStuff : MonoBehaviour
    {
        public static bool LowerBarsHidden;
        public static bool MiniMapHidden;
        [SerializeField] Toggle hideLowerBars;
        [SerializeField] Toggle hideMap;

        void Start()
        {
            hideLowerBars.SetIsOnWithoutNotify(LowerBarsHidden);
            hideLowerBars.onValueChanged.AddListener(ToggleHealthBars);

            hideMap.SetIsOnWithoutNotify(MiniMapHidden);
            hideMap.onValueChanged.AddListener(ToggleMap);
        }

        static void ToggleHealthBars(bool arg0) => LowerBarsHidden = arg0;
        static void ToggleMap(bool arg0) => MiniMapHidden = arg0;
    }
}