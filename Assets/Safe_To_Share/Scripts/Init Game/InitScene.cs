using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Safe_To_Share.Scripts.Farming;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Init_Game {
    public sealed class InitScene : MonoBehaviour {
        static bool init;

        [SerializeField] GameObject[] dontDestroyObjects;
        [SerializeField] List<AssetReference> assetRefs = new();

        // Start is called before the first frame update
        IEnumerator Start() {
            if (init) {
                Destroy(gameObject);
                yield break;
            }

            init = true;

            FarmAreas.Initialize();
            foreach (var toAdd in dontDestroyObjects)
                DontDestroyOnLoad(Instantiate(toAdd));
            yield return InstanceAssets();
        }

        IEnumerator InstanceAssets() {
            var ops = assetRefs.Select(asset => asset.InstantiateAsync()).ToArray();
            while (StillLoadingOperations(ops))
                yield return null;
            foreach (var operation in ops)
                DontDestroyOnLoad(operation.Result);
        }

        static bool StillLoadingOperations(IEnumerable<AsyncOperationHandle<GameObject>> ops) =>
            ops.Any(operation => operation.Status != AsyncOperationStatus.Succeeded);
    }
}