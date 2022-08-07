using Character.PlayerStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public class FarmCanvas : MonoBehaviour
    {
        [SerializeField] ShowPlantOptions showPlantOptions;
        public void Open(Player player)
        {
            gameObject.SetActive(true);
            showPlantOptions.Setup(player.Inventory);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}