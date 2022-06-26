using DormAndHome.Dorm;
using DormAndHome.Dorm.Buildings;
using UnityEngine;

namespace DormAndHome
{
    public class DormHasBuildings : MonoBehaviour
    {
        [SerializeField] GameObject[] dormLodges;
        [SerializeField] GameObject[] kitchen;
        [SerializeField] GameObject[] gym;
        [SerializeField] GameObject[] essenceStone;
        DormBuildings dormBuildings;

        void Start()
        {
            dormBuildings = DormManager.Instance.Buildings;
            ShowOwnedBuildings();
            DormManager.Loaded += ShowOwnedBuildings;
            DormManager.Instance.Buildings.Gym.Upgraded += CheckGym;
            DormManager.Instance.Buildings.Kitchen.Upgraded += CheckKitchen;
            DormManager.Instance.Buildings.DormLodge.Upgraded += CheckLodge;
            DormManager.Instance.Buildings.DormLodge.EssenceStone.Upgraded += CheckEssStone;
        }

        void OnDestroy()
        {
            DormManager.Loaded -= ShowOwnedBuildings;
            DormManager.Instance.Buildings.Gym.Upgraded -= CheckGym;
            DormManager.Instance.Buildings.Kitchen.Upgraded -= CheckKitchen;
            DormManager.Instance.Buildings.DormLodge.Upgraded -= CheckLodge;
            DormManager.Instance.Buildings.DormLodge.EssenceStone.Upgraded -= CheckEssStone;
        }

        static void SetActiveBuildingTier(GameObject[] array, int tier)
        {
            if (array.Length <= 0)
                return;
            foreach (GameObject o in array)
                o.SetActive(false);
            int index = tier - 1;
            if (index >= 0)
                array[Mathf.Min(index, array.Length - 1)].SetActive(true);
        }

        void ShowOwnedBuildings()
        {
            CheckLodge();
            CheckKitchen();
            CheckGym();
            CheckEssStone();
        }

        void CheckEssStone() => SetActiveBuildingTier(essenceStone, dormBuildings.DormLodge.EssenceStone.Level);

        void CheckGym() => SetActiveBuildingTier(gym, dormBuildings.Gym.Level);

        void CheckKitchen() => SetActiveBuildingTier(kitchen, dormBuildings.Kitchen.Level);

        void CheckLodge() => SetActiveBuildingTier(dormLodges, dormBuildings.DormLodge.Level);
    }
}