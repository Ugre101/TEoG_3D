using AvatarStuff.Holders;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts
{
    public sealed class GainPerkCollider : MonoBehaviour
    {
        [SerializeField] AssetReference perkGuid;

        MovementPerk loaded;

        void Start()
        {
            if (perkGuid.RuntimeKeyIsValid())
                perkGuid.LoadAssetAsync<MovementPerk>().Completed += Done;
            else
                gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !other.TryGetComponent(out PlayerHolder playerHolder) ||
                playerHolder.Player.LevelSystem.OwnedPerks.Contains(loaded))
                return;
            loaded.GainPerk(playerHolder);
            EventLog.AddEvent($"Allowing you to {loaded.Desc}");
            EventLog.AddEvent($"You gained the {loaded.Title} perk.");
        }

        void Done(AsyncOperationHandle<MovementPerk> obj) => loaded = obj.Result;
    }
}