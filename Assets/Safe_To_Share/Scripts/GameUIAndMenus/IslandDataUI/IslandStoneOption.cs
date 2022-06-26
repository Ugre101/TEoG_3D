using AvatarStuff.Holders;
using Character.IslandData;
using Character.PlayerStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.IslandDataUI
{
    public abstract class IslandStoneOption : MonoBehaviour
    {
        [SerializeField] protected PlayerHolder playerHolder;
        [SerializeField] protected Islands island;
        [SerializeField] protected TextMeshProUGUI currentAmount;
        [SerializeField] protected Button increaseButton, decreaseQuestButton;
    }
}