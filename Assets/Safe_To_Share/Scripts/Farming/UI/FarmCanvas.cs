using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public class FarmCanvas : MonoBehaviour
    {
        [SerializeField] ShowPlantOptions showPlantOptions;
        public void Open(Player player)
        {
            gameObject.SetActive(true);
            GameUIManager.TriggerHideGameUI(true);
            showPlantOptions.Setup(player.Inventory);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            GameUIManager.TriggerHideGameUI(false);
        }
    }
}