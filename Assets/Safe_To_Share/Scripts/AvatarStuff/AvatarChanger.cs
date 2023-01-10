using System;
using Movement.ECM2.Source;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AvatarStuff
{
    public class AvatarChanger : AvatarChangerBase
    {
        bool hasAvatar;
        public bool AvatarLoaded { get; private set; }
        public CharacterAvatar CurrentAvatar { get; private set; }
        public event Action<CharacterAvatar> NewAvatar;
        public UnityEvent<CharacterAvatar> NewAvatarEvent;

        public void UpdateAvatar(AssetReference avatar)
        {
            if (hasAvatar && CurrentAvatar.Prefab.AssetGUID == avatar.AssetGUID)
            {
                NewAvatar?.Invoke(CurrentAvatar);
                return;
            }

            AvatarLoaded = false;
            avatar.InstantiateAsync(transform).Completed += Done;
            //    avatar.LoadAssetAsync<GameObject>().Completed += Done;
        }

        void Done(AsyncOperationHandle<GameObject> obj)
        {
            if (hasAvatar)
                Destroy(CurrentAvatar.gameObject);
            if (obj.Result == null)
                return;
            AvatarLoaded = true;
            if (obj.Result.TryGetComponent(out Animator ani))
                InvokeNewAnimator(ani);
            if (obj.Result.TryGetComponent(out CharacterAvatar avatar))
            {
                CurrentAvatar = avatar;
                hasAvatar = true;
                NewAvatar?.Invoke(avatar);
            }
        }

        public void UpdateAvatar(GameObject value)
        {
            if (!value.TryGetComponent(out CharacterAvatar avatar)) return;
            if (hasAvatar && CurrentAvatar.Prefab.AssetGUID == avatar.Prefab.AssetGUID)
            {
                NewAvatar?.Invoke(CurrentAvatar);
                NewAvatarEvent?.Invoke(CurrentAvatar);
                return;
            }

            AvatarLoaded = false;
            if (hasAvatar)
                Destroy(CurrentAvatar.gameObject);
            var instance = Instantiate(avatar, transform);
            instance.transform.localPosition = Vector3.zero;
            AvatarLoaded = true;
            CurrentAvatar = instance;
            hasAvatar = true;
            if (instance.TryGetComponent(out Animator ani))
                InvokeNewAnimator(ani);
            NewAvatar?.Invoke(instance);
            NewAvatarEvent?.Invoke(instance);
            //    avatar.LoadAssetAsync<GameObject>().Completed += Done;
        }
    }
}