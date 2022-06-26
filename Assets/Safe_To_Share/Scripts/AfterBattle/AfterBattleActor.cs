using AvatarStuff;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleActor : MonoBehaviour
    {
        static readonly int Idle = Animator.StringToHash("AfterBattle");
        [SerializeField] AvatarInfoDict avatarDict;
        [SerializeField] AvatarChanger avatarChanger;
        [SerializeField] AfterBattleAvatarScaler avatarScaler;
        [SerializeField] bool playerAvatar;
        Animator animator;
        CharacterAvatar avatar;

        AvatarInfo currentInfo;
        bool hasAvatar;
        AddedAnimations.SexAnimations? lastAnimation;

        public BaseCharacter Actor { get; private set; }

        void OnDestroy() => UnSub();

        void UnSub()
        {
            avatarChanger.NewAnimator -= NewAnimator;
            avatarChanger.NewAvatar -= NewAvatar;
            if (Actor == null)
                return;
            Actor.UpdateAvatar -= ModifyAvatar;
            Actor.Body.Height.StatDirtyEvent -= UpdateHeight;
            Actor.SexStats.ArousalChange -= UpdateArousal;
        }

        void UpdateArousal(int obj)
        {
            if (hasAvatar)
                avatar.SetArousal(obj);
        }

        void NewAvatar(CharacterAvatar obj)
        {
            avatar = obj;
            hasAvatar = true;
            currentInfo = avatarDict.GetInfo(Actor);
            avatar.Setup(Actor);
            UpdateHeight();
            avatar.SetArousal(Actor.SexStats.Arousal);
        }

        public async void ModifyAvatar()
        {
            if (hasAvatar && currentInfo == avatarDict.GetInfo(Actor))
            {
                UpdateCurrentAvatar();
                return;
            }

            hasAvatar = false;
            var res = await avatarDict.GetAvatarLoaded(Actor, playerAvatar);
            avatarChanger.UpdateAvatar(res);
        }

        public void UpdateCurrentAvatar()
        {
            if (!hasAvatar) return;
            avatar.Setup(Actor);
            UpdateHeight();
            avatar.SetArousal(Actor.SexStats.Arousal);
        }

        public void UpdateHeight() => avatarScaler.ChangeScale(Actor.Body.Height.Value);

        void NewAnimator(Animator obj)
        {
            animator = obj;
            animator.SetBool(Idle, true);
        }

        public void Setup(BaseCharacter character)
        {
            Actor = character;
            UnSub();
            Actor.UpdateAvatar += ModifyAvatar;
            Actor.Body.Height.StatDirtyEvent += UpdateHeight;
            avatarChanger.NewAnimator += NewAnimator;
            avatarChanger.NewAvatar += NewAvatar;
            Actor.SexStats.ArousalChange += UpdateArousal;
            ModifyAvatar();
            //  avatarChanger.UpdateAvatar(avatarDict.GetAvatar(Actor,false));
        }

        public void SetActAnimation(AddedAnimations.SexAnimations ani)
        {
            if (lastAnimation.HasValue)
                animator.SetBool(AddedAnimations.GetAnimationHash[lastAnimation.Value], false);
            if (!AddedAnimations.GetAnimationHash.TryGetValue(ani, out int hash))
                return;
            animator.SetBool(hash, true);
            lastAnimation = ani;
        }

        public void Removed() => avatarChanger.gameObject.SetActive(false);
    }
}