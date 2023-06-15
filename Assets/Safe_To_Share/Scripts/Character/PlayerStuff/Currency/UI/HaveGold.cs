using TMPro;
using UnityEngine;

namespace Currency.UI {
    public sealed class HaveGold : MonoBehaviour {
        [SerializeField] TextMeshProUGUI goldAmount;

        public void GoldChanged(int obj) => goldAmount.text = $"Gold\n{obj}";
    }
}