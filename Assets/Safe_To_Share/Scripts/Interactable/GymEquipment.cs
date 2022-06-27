using System.Linq;
using System.Text;
using Character.BodyStuff;
using Character.PlayerStuff;
using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Interactable
{
    public class GymEquipment : MonoBehaviour, IInteractable
    {
        [SerializeField] Item[] needItem;
        [SerializeField] string title;
        [SerializeField] string needText;
        [SerializeField] bool trainMuscle;
        [SerializeField, Range(3f, 20f),] float intensityFat = 2f;
        [SerializeField, Range(1f, 3f),] float intensityMuscle = 1f;

        void Start()
        {
            if (needItem == null)
            {
                gameObject.SetActive(false);
                Debug.LogError("Gym equipment is missing it's needed item");
            }
        }

        public string HoverText(Player player) => needItem.Any(item => player.Inventory.HasItemOfGuid(item.Guid)) ? title : needText;

        public void DoInteraction(Player player)
        {
            foreach (Item item in needItem)
                if (player.Inventory.HasItemOfGuid(item.Guid))
                {
                    UseItemAndDoAction(player, item.Guid);
                    break;
                }
        }
        void UseItemAndDoAction(Player player, string guid)
        {
            player.Inventory.UseItemItemID(guid);
            StringBuilder returnText = new("You spent an hour working out and ");
            if (trainMuscle)
            {
                returnText.Append($"gained {BodyExtensions.Train(player.Body, intensityMuscle).ConvertKg()} of muscle and ");
            }
            returnText.Append($"burned {player.Body.BurnFatHour(1,intensityFat).ConvertKg()} of fat.");
            EventLog.AddEvent(returnText.ToString());
            player.InvokeUpdateAvatar();
            DateSystem.PassHour();
        }

    }
}