using System.Threading.Tasks;
using Character.GenderStuff;
using Character.Race.Races;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AvatarStuff
{
    [CreateAssetMenu(fileName = "New avatar Info", menuName = "Character/Avatar/Avatar Info", order = 0)]
    public class AvatarInfo : ScriptableObject
    {
        [SerializeField] AssetReference prefab;
        [SerializeField] AssetReference playerPrefab;
        [SerializeField] Gender[] supportedGenders;
        [SerializeField] BasicRace[] supportedRaces;
        GameObject loaded;

        bool loading,playerLoading, done,playerDone;
        GameObject playerLoaded;
        AsyncOperationHandle<GameObject> playerOp;

        public Gender[] SupportedGenders => supportedGenders;
        public BasicRace[] SupportedRaces => supportedRaces;

        public AssetReference Prefab => prefab;

        public AssetReference PlayerPrefab => playerPrefab;

        public async Task<GameObject> GetLoadedPrefab(bool player)
        {
            if (player)
            {
                if (playerLoaded != null)
                    return playerLoaded;
                if (playerLoading)
                {
                    while (!playerDone)
                    {
                        await Task.Delay(100);
                        return loaded;
                    }
                }
                playerLoading = true;
                playerOp = playerPrefab.LoadAssetAsync<GameObject>();
                await playerOp.Task;
                if (playerOp.Task.IsCompletedSuccessfully)
                {
                    playerLoaded = playerOp.Task.Result;
                    playerDone = true;
                    return playerLoaded;
                }
            }

            if (loaded != null)
                return loaded;
            if (loading)
            {
                while (!done) await Task.Delay(100);

                return loaded;
            }

            loading = true;
            var op = prefab.LoadAssetAsync<GameObject>();
            await op.Task;
            loaded = op.Result;
            done = true;
            return loaded;
        }

        public void UnLoadPlayer()
        {
            if (!playerOp.IsValid()) return;
            Debug.Log("Unloaded old player avatar");
            Addressables.Release(playerOp);
        }
    }
}