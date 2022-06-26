using Character;
using UnityEngine;

namespace AvatarStuff.Holders
{
    public abstract class Holder : MonoBehaviour
    {
        [SerializeField] protected AvatarInfoDict avatarDict;
        [SerializeField] protected AvatarChanger avatarChanger;
        [SerializeField] protected AvatarScaler avatarScaler;
        [SerializeField] protected bool playerAvatar;
        public CharacterAvatar CurrentAvatar { get; protected set; }

        public AvatarScaler Scaler => avatarScaler;

        protected virtual async void UpdateAvatar(BaseCharacter whom)
        {
            var res = await avatarDict.GetAvatarLoaded(whom, playerAvatar);
            avatarChanger.UpdateAvatar(res);
        }

        public virtual void HeightsChange(float newHeight) => Scaler.ChangeScale(newHeight);

        protected virtual void Sub() => avatarChanger.NewAvatar += NewAvatar;

        protected virtual void UnSub() => avatarChanger.NewAvatar -= NewAvatar;

        protected abstract void NewAvatar(CharacterAvatar obj);
        
    }
}