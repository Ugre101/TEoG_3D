using AvatarStuff.Holders;
using Character.PlayerStuff;
using SaveStuff;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneStuff
{
    public class LoadSubSceneWhenClose : MonoBehaviour
    {
        [SerializeField] protected SubLocationSceneSo subScene;
        [SerializeField] float loadDist = 150;
        [SerializeField] Collider blockingCollider;
        private bool hasBlockingObejct;
        float lastTick;
        Transform player;

        protected virtual void Start()
        {
            hasBlockingObejct = blockingCollider != null;
            lastTick = Time.time;
            var playerHolder = PlayerHolder.Instance;
            if (playerHolder != null)
                player = playerHolder.transform;
            subScene.Activated += LoadedMe;
        }

        protected virtual void Update()
        {
            if (Time.time < lastTick + 0.5f )
                return;
            lastTick = Time.time;
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist <= loadDist)
                IfNotActiveLoadScene();
            else if (loadDist + 5f < dist) 
                IfActiveUnloadScene();
        }

        private void IfNotActiveLoadScene()
        {
            if (subScene.SceneActive || subScene.SceneLoaded)
                return;
            LoadScene();
        }

        protected virtual void OnDestroy()
        {
            IfActiveUnloadScene();
            subScene.SceneActive = false;
            subScene.SceneLoaded = false;
            subScene.Activated -= LoadedMe;
        }

        private void IfActiveUnloadScene()
        {
            if (!subScene.SceneActive || !subScene.SceneLoaded)
                return;
            UnLoadScene();
        }
        
        protected virtual void LoadedMe()
        {
            if (subScene.SceneActive)
                ToggleActiveBlockingObject(false);
        }

        protected void UnLoadScene()
        {
            subScene.SceneActive = false;
            ToggleActiveBlockingObject(true);
            subScene.SceneReference.UnLoadScene().Completed += DoneUnloading;
        }

        void DoneUnloading(AsyncOperationHandle<SceneInstance> obj) => subScene.SceneLoaded = false;

        void LoadScene()
        {
            subScene.SceneActive = true;
            subScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).Completed += Open;
        }

        void Open(AsyncOperationHandle<SceneInstance> obj)
        {
            subScene.SceneLoaded = true;
            ToggleActiveBlockingObject(false);
        }

        private void ToggleActiveBlockingObject(bool value)
        {
            if (hasBlockingObejct)
                blockingCollider.gameObject.SetActive(value);
        }
    }
}