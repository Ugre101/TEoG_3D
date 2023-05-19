using Character.Organs;
using Character.VoreStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public sealed class VoreMenu : GameMenu
    {
        [SerializeField] StomachVoreOrganContainerInfo stomach;
        [SerializeField] SexualVoreOrganContainerInfo cock, balls, anal, boobs, vagina;
        [SerializeField] GameObject start, perk;
        [SerializeField] VoreOrganSettings voreOrganSettings;

        void OnEnable()
        {
            start.gameObject.SetActive(true);
            perk.gameObject.SetActive(false);
            Setup();
        }

        void Setup()
        {
            stomach.Setup("Stomach", Player.Vore.Stomach, VoreSystemExtension.OralVoreCapacity(Player));
            cock.Setup(Player, SexualOrganType.Dick);
            balls.Setup(Player, SexualOrganType.Balls);
            anal.Setup(Player, SexualOrganType.Anal);
            boobs.Setup(Player, SexualOrganType.Boobs);
            vagina.Setup(Player, SexualOrganType.Vagina);
            voreOrganSettings.Setup(Player);
        }
    }
}