using System;
using System.Threading.Tasks;
using Movement.ECM2.Source;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AvatarStuff
{
    public class AvatarChanger : AvatarChangerBase
    {
        public event Action<CharacterAvatar> NewAvatar;
        public bool AvatarLoaded { get; private set; }
        CharacterAvatar currentAvatar;
        bool hasAvatar;
        public void UpdateAvatar(AssetReference avatar)
        {

            if (hasAvatar && currentAvatar.Prefab.AssetGUID == avatar.AssetGUID)
            {
                NewAvatar?.Invoke(currentAvatar);
                return;
            }
            AvatarLoaded = false;
            avatar.InstantiateAsync(transform).Completed += Done;
            //    avatar.LoadAssetAsync<GameObject>().Completed += Done;
        }

        void Done(AsyncOperationHandle<GameObject> obj)
        {
            if (hasAvatar)
                Destroy(currentAvatar.gameObject);
            if (obj.Result == null)
                return;
            AvatarLoaded = true;
            if (obj.Result.TryGetComponent(out Animator ani))
                InvokeNewAnimator(ani);
            if (obj.Result.TryGetComponent(out CharacterAvatar avatar))
            {
                NewAvatar?.Invoke(avatar);
                currentAvatar = avatar;
                hasAvatar = true;
            }
        }

        public void UpdateAvatar(GameObject value)
        {
            if(!value.TryGetComponent(out CharacterAvatar avatar)) return;
            if (hasAvatar && currentAvatar.Prefab.AssetGUID == avatar.Prefab.AssetGUID)
            {
                NewAvatar?.Invoke(currentAvatar);
                return;
            }
            AvatarLoaded = false;
            if (hasAvatar)
                Destroy(currentAvatar.gameObject);
            var instance = Instantiate(avatar, transform);
            AvatarLoaded = true;
            if (instance.TryGetComponent(out Animator ani))
                InvokeNewAnimator(ani);
            NewAvatar?.Invoke(instance);
            currentAvatar = instance;
            hasAvatar = true;
            //    avatar.LoadAssetAsync<GameObject>().Completed += Done;
        }

    }
}