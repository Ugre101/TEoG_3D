using System;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace GameUIAndMenus.Menus.Vore
{
    public class SexualVoreOrganContainerInfo : VoreOrganContainerInfo
    {
        public static event Action<SexualOrganType> ShowOrganSettingForMe; 
        [SerializeField] SexualVoreOrganCapacityInfo prefab;

        SexualOrganType voreOrganType;
        public void Setup(BaseCharacter pred, SexualOrganType organType)
        {
            voreOrganType = organType;
            putHere.KillChildren();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ShowMe);
            if (!pred.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer container))
                return;
            foreach (BaseOrgan baseOrgan in container.List)
                Instantiate(prefab, putHere).Setup(pred, organType, baseOrgan);
        }

        protected override void ShowMe() => ShowOrganSettingForMe?.Invoke(voreOrganType);
    }
}