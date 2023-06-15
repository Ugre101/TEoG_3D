using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Safe_To_Share.Scripts.StartScene {
    public sealed class LoadStartScene : MonoBehaviour {
        [SerializeField] AssetReference startMenu;

        // Start is called before the first frame update
        void Start() => startMenu.LoadSceneAsync();
    }
}