using System.Collections;
using System.Collections.Generic;
using DormAndHome.Dorm;
using DormAndHome.Dorm.Buildings;
using UnityEngine;

namespace DormAndHome
{
    public sealed class DormVillageExpansionHaveBuildings : MonoBehaviour
    {
        [SerializeField] List<GameObject> brothels = new();

        readonly WaitForSeconds waitForSeconds = new(0.1f);
        DormBuildings dormBuildings;

        IEnumerator Start()
        {
            dormBuildings = DormManager.Instance.Buildings;
            DormManager.Loaded += ShowOwnedBuildings;
            DormManager.Instance.Buildings.VillageBuildings.Brothel.Upgraded += CheckBrothel;
            yield return waitForSeconds;
            ShowOwnedBuildings();
        }

        void OnDestroy()
        {
            DormManager.Loaded -= ShowOwnedBuildings;
            DormManager.Instance.Buildings.VillageBuildings.Brothel.Upgraded -= CheckBrothel;
        }

        static void SetActiveBuildingTier(List<GameObject> array, int tier)
        {
            if (array.Count <= 0)
                return;
            if (tier <= 0)
                foreach (GameObject o in array)
                    o.SetActive(false);
            for (int i = 0; i < array.Count; i++)
                array[i].SetActive(i == tier - 1);
        }

        void ShowOwnedBuildings() => CheckBrothel();

        void CheckBrothel() => SetActiveBuildingTier(brothels, dormBuildings.VillageBuildings.Brothel.Level);
    }
}