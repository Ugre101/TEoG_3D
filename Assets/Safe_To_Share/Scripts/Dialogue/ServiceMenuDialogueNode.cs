using Character.Service;
using UnityEngine;

namespace Dialogue
{
    public class ServiceMenuDialogueNode : DialogueBaseNode
    {
        [SerializeField] OfferServices offerServices;
        [SerializeField] string shopTitle;


        public string ShopTitle => shopTitle;

        public OfferServices Services => offerServices;
    }
}