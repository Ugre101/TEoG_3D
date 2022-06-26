using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Init_Game
{
    public class InitScene : MonoBehaviour
    {
        static bool init;

        [SerializeField] GameObject[] dontDestroyObjects;
        [SerializeField] List<AssetReference> assetRefs = new();

        // Start is called before the first frame update
        void Start()
        {
            if (init)
            {
                Destroy(gameObject);
                return;
            }
            init = true;

            foreach (GameObject toAdd in dontDestroyObjects)
                DontDestroyOnLoad(Instantiate(toAdd));
            StartCoroutine(InstanceAssets());
        }

        private IEnumerator InstanceAssets()
        {
            var ops = assetRefs.Select(asset => asset.InstantiateAsync()).ToArray();
            while (StillLoadingOperations(ops))
                yield return null;
            foreach (var operation in ops)
                DontDestroyOnLoad(operation.Result);
        }

        private static bool StillLoadingOperations(IEnumerable<AsyncOperationHandle<GameObject>> ops) 
            => ops.Any(operation => operation.Status != AsyncOperationStatus.Succeeded);
    }
}