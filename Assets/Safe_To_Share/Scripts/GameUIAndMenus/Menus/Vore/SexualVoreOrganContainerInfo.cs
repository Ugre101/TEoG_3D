using System;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public sealed class SexualVoreOrganContainerInfo : VoreOrganContainerInfo
    {
        [SerializeField] SexualVoreOrganCapacityInfo prefab;

        SexualOrganType voreOrganType;
        public static event Action<SexualOrganType> ShowOrganSettingForMe;

        public void Setup(BaseCharacter pred, SexualOrganType organType)
        {
            voreOrganType = organType;
            putHere.KillChildren();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ShowMe);
            if (!pred.SexualOrgans.Containers.TryGetValue(organType, out BaseOrgansContainer container))
                return;
            foreach (BaseOrgan baseOrgan in container.BaseList)
                Instantiate(prefab, putHere).Setup(pred, organType, baseOrgan);
        }

        protected override void ShowMe() => ShowOrganSettingForMe?.Invoke(voreOrganType);
    }
}