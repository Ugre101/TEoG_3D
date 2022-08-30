using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI
{
    public class CraftingResult : MonoBehaviour
    {
        [SerializeField] Sprite questionMark;
        [SerializeField] Image image;

        void OnEnable()
        {
            image.sprite = questionMark;
        }

        public void AddResult(Item opResult)
        {
            image.sprite = opResult.Icon;
            // If already has old result first add that to inventory?    
        }

        public bool StillHasItem(out Item o, out int i)
        {
            o = null;
            i = 1;
            return false;
        }

        public void ShowQuestionMark()
        {
            image.sprite = questionMark;
        }
    }
}