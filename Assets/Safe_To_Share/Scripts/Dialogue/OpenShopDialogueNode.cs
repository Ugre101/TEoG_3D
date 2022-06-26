using Character.Shop;
using UnityEngine;

namespace Dialogue
{
    public class OpenShopDialogueNode : DialogueBaseNode
    {
        [SerializeField] ShopItems shopItems;
        [SerializeField] string shopTitle;

        public ShopItems ShopItems => shopItems;

        public string ShopTitle => shopTitle;
    }
}