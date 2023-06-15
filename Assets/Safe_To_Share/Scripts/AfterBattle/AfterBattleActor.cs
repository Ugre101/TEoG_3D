﻿using AvatarStuff;
using Character;
using Safe_To_Share.Scripts.AfterBattle.Actor;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle {
    public sealed class AfterBattleActor : MonoBehaviour {
        [SerializeField] AvatarInfoDict avatarDict;
        [SerializeField] AvatarChanger avatarChanger;
        [SerializeField] bool playerAvatar;

        [field: SerializeField] public AfterBattleAvatarScaler AvatarScaler { get; private set; }
        [field: SerializeField] public RotateActor RotateActor { get; private set; }
        [field: SerializeField] public AlignActor AlignActor { get; private set; }
        readonly SexAnimationManager sexAnimationManager = new();
        Animator animator;


        RuntimeAnimatorController animatorController;
        AvatarInfo currentInfo;
        AddedAnimations.SexAnimations? lastAnimation;

        Vector3 startPos;
        Quaternion startRot;
        public bool HasAvatar { get; private set; }

        public CharacterAvatar Avatar { get; private set; }

        public BaseCharacter Actor { get; private set; }


        void OnDestroy() {
            UnSub();
        }

        void UnSub() {
            avatarChanger.NewAvatar -= NewAvatar;
            if (Actor == null)
                return;
            Actor.UpdateAvatar -= ModifyAvatar;
            Actor.Body.Height.StatDirtyEvent -= UpdateHeight;
            Actor.SexStats.ArousalChange -= UpdateArousal;
        }

        void UpdateArousal(int obj) {
            if (HasAvatar)
                Avatar.SetArousal(obj);
        }

        void NewAvatar(CharacterAvatar obj) {
            Avatar = obj;

            HasAvatar = true;
            currentInfo = avatarDict.GetInfo(Actor);
            Avatar.Setup(Actor);
            UpdateHeight();
            Avatar.SetArousal(Actor.SexStats.Arousal);
            obj.Animator.runtimeAnimatorController = animatorController;
            AlignActor.NewAvatar(false);
        }

        public async void ModifyAvatar() {
            if (HasAvatar && currentInfo == avatarDict.GetInfo(Actor)) {
                UpdateCurrentAvatar();
                return;
            }

            AlignActor.NewAvatar(true);
            HasAvatar = false;
            var res = await avatarDict.GetAvatarLoaded(Actor, playerAvatar);
            avatarChanger.UpdateAvatar(res);
        }

        void UpdateCurrentAvatar() {
            if (!HasAvatar) return;
            Avatar.Setup(Actor);
            UpdateHeight();
            Avatar.SetArousal(Actor.SexStats.Arousal);
        }

        void UpdateHeight() => AvatarScaler.ChangeScale(Actor.Body.Height.Value, playerAvatar);

        public void NewAnimator(Animator obj) {
            sexAnimationManager.Clear();
            animator = obj;
        }

        public void Setup(BaseCharacter character, RuntimeAnimatorController controller) {
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