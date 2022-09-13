using System.Threading.Tasks;
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

        public AvatarScaler Scaler => avatarScaler;

        public AvatarChanger Changer => avatarChanger;

        protected virtual async Task UpdateAvatar(BaseCharacter whom)
        {
            var res = await avatarDict.GetAvatarLoaded(whom, playerAvatar);
            Changer.UpdateAvatar(res);
        }

        public virtual void HeightsChange(float newHeight) => Scaler.ChangeScale(newHeight);

        protected virtual void Sub() => Changer.NewAvatar += NewAvatar;

        protected virtual void UnSub() => Changer.NewAvatar -= NewAvatar;

        protected abstract void NewAvatar(CharacterAvatar obj);
    }
}