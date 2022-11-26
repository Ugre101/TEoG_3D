using System;
using AvatarStuff;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleActor : MonoBehaviour
    {
        [SerializeField] AvatarInfoDict avatarDict;
        [SerializeField] AvatarChanger avatarChanger;
        [SerializeField] AfterBattleAvatarScaler avatarScaler;
        [SerializeField] bool playerAvatar;
        readonly SexAnimationManager sexAnimationManager = new();
        Animator animator;


        RuntimeAnimatorController animatorController;
        AvatarInfo currentInfo;
        bool hasAvatar;
        AddedAnimations.SexAnimations? lastAnimation;

        Vector3 startPos;
        Quaternion startRot;

        public CharacterAvatar Avatar { get; private set; }

        public BaseCharacter Actor { get; private set; }

        [field: SerializeField] public RotateActor RotateActor { get; private set; }

        

        void OnDestroy()
        {
            UnSub();
        }

        void UnSub()
        {
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
                Avatar.SetArousal(obj);
        }

        public void NewAvatar(CharacterAvatar obj)
        {
            Avatar = obj;

            hasAvatar = true;
            currentInfo = avatarDict.GetInfo(Actor);
            Avatar.Setup(Actor);
            UpdateHeight();
            Avatar.SetArousal(Actor.SexStats.Arousal);
            obj.Animator.runtimeAnimatorController = animatorController;
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
            Avatar.Setup(Actor);
            UpdateHeight();
            Avatar.SetArousal(Actor.SexStats.Arousal);
        }

        public void UpdateHeight() => avatarScaler.ChangeScale(Actor.Body.Height.Value);

        public void NewAnimator(Animator obj)
        {
            sexAnimationManager.Clear();
            animator = obj;
        }

        public void Setup(BaseCharacter character, RuntimeAnimatorController controller)
        {
            Actor = character;
            animatorController = controller;
            UnSub();
            Actor.UpdateAvatar += ModifyAvatar;
            Actor.Body.Height.StatDirtyEvent += UpdateHeight;
            avatarChanger.NewAvatar += NewAvatar;
            Actor.SexStats.ArousalChange += UpdateArousal;
            ModifyAvatar();
            //  avatarChanger.UpdateAvatar(avatarDict.GetAvatar(Actor,false));
        }

        public void SetActAnimation(int hash) => sexAnimationManager.TryPlayAnimation(animator, hash);
        public void Removed() => avatarChanger.gameObject.SetActive(false);
    }
}