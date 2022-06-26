using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Safe_To_Share.Scripts.StartScene
{
    public class LoadStartScene : MonoBehaviour
    {
        [SerializeField] AssetReference startMenu;
        // Start is called before the first frame update
        void Start() => startMenu.LoadSceneAsync();
    }
}
