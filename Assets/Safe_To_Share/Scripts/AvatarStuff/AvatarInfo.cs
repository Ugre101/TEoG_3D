using System;
using System.Threading.Tasks;
using Character.GenderStuff;
using Character.Race.Races;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AvatarStuff
{
    [CreateAssetMenu(fileName = "New avatar Info", menuName = "Character/Avatar/Avatar Info", order = 0)]
    public sealed class AvatarInfo : ScriptableObject
    {
        [SerializeField] AssetReference prefab;
        [SerializeField] AssetReference playerPrefab;
        [SerializeField] Gender[] supportedGenders;
        [SerializeField] BasicRace[] supportedRaces;

        AsyncOperationHandle<GameObject> playerOp;
        AsyncOperationHandle<GameObject> avatarOp;

        public Gender[] SupportedGenders => supportedGenders;
        public BasicRace[] SupportedRaces => supportedRaces;

        public AssetReference Prefab => prefab;

        public AssetReference PlayerPrefab => playerPrefab;

        public async Task<GameObject> GetLoadedPrefab(bool player)
        {
            if (player)
                return await PlayerLoaded();

            if (avatarOp.IsValid() && avatarOp is { IsDone: true, Result: { } loadedPrefab })
            {
                return loadedPrefab;
            }
            if (avatarOp.IsValid())
            {
                while (!avatarOp.IsDone) 
                    await Task.Delay(100);
                if (avatarOp.Result is { } result)
                    return result;
            }

            avatarOp = prefab.LoadAssetAsync<GameObject>();
            await avatarOp.Task;
            return avatarOp.Result;
        }

        async Task<GameObject> PlayerLoaded()
        {
            if (playerOp.IsValid() && playerOp is {IsDone: true, Result: {} playerLoaded})
                return playerLoaded;
            if (playerOp.IsValid())
            {
                while (!playerOp.IsDone) 
                    await Task.Delay(100);
                if (avatarOp.Result is {} doneLoading)
                    return doneLoading;
            }

            playerOp = playerPrefab.LoadAssetAsync<GameObject>();
            await playerOp.Task;
            if (!playerOp.Task.IsCompletedSuccessfully)
                throw new NullReferenceException("Failed to load player");
            return playerOp.Result;
        }

        public void UnLoadPlayer()
        {
            if (!playerOp.IsValid()) return;
            Addressables.Release(playerOp);
        }
    }
}