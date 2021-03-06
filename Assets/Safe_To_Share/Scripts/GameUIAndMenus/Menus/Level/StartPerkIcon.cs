using Character.LevelStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.Menus.Level
{
    public class StartPerkIcon : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Image icon;
        BasicPerk perk;

        public void Setup(BasicPerk startPerk)
        {
            perk = startPerk;
            title.text = startPerk.Title;
            icon.sprite = startPerk.Icon;
        }
    }
}