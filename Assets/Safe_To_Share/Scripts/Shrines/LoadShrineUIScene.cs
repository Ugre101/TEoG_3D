using System.Collections;
using Character.PlayerStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Safe_To_Share.Scripts.Shrines {
    public sealed class LoadShrineUIScene : MonoBehaviour {
        [SerializeField] AssetReference uiScene;


        public void LoadUIScene(Player player) {
            StartCoroutine(Load(player));
        }

        public void UnLoad() {
            uiScene.UnLoadScene();
        }

        IEnumerator Load(Player player) {
            var op = uiScene.LoadSceneAsync(LoadSceneMode.Additive);
            yield return op;
            yield return null; // Wait a frame
            if (BaseShrine.LastLoadedShrine != null)
                BaseShrine.LastLoadedShrine.EnterShrine(player);
        }
    }
}