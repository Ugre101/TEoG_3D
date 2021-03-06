using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class HideGameUIStuff : MonoBehaviour
    {
        public static bool LowerBarsHidden = false;
        public static bool MiniMapHidden = false;
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