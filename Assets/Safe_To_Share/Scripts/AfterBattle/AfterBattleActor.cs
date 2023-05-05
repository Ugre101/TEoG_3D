using System.Threading.Tasks;
using AvatarStuff;
using Character;
using Safe_To_Share.Scripts.AfterBattle.Actor;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleActor : MonoBehaviour
    {
        [SerializeField] AvatarInfoDict avatarDict;
        [SerializeField] AvatarChanger avatarChanger;
        [SerializeField] bool playerAvatar;
        readonly SexAnimationManager sexAnimationManager = new();
        Animator animator;


        RuntimeAnimatorController animatorController;
        AvatarInfo currentInfo;
        public bool HasAvatar { get; private set; }
        AddedAnimations.SexAnimations? lastAnimation;

        Vector3 startPos;
        Quaternion startRot;

        public CharacterAvatar Avatar { get; private set; }

        public BaseCharacter Actor { get; private set; }

        [field: SerializeField] public AfterBattleAvatarScaler AvatarScaler { get; private set; }
        [field: SerializeField] public RotateActor RotateActor { get; private set; }
        [field: SerializeField] public AlignActor AlignActor { get; private set; }
        

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
            if (HasAvatar)
                Avatar.SetArousal(obj);
        }

        public void NewAvatar(CharacterAvatar obj)
        {
            Avatar = obj;

            HasAvatar = true;
            currentInfo = avatarDict.GetInfo(Actor);
            Avatar.Setup(Actor);
            UpdateHeight();
            Avatar.SetArousal(Actor.SexStats.Arousal);
            obj.Animator.runtimeAnimatorController = animatorController;
        }

        public async void ModifyAvatar()
        {
            if (HasAvatar && currentInfo == avatarDict.GetInfo(Actor))
            {
                UpdateCurrentAvatar();
                return;
            }

            HasAvatar = false;
            var res = await avatarDict.GetAvatarLoaded(Actor, playerAvatar);
            avatarChanger.UpdateAvatar(res);
        }

        public void UpdateCurrentAvatar()
        {
            if (!HasAvatar) return;
            Avatar.Setup(Actor);
            UpdateHeight();
            Avatar.SetArousal(Actor.SexStats.Arousal);
        }

        public void UpdateHeight() => AvatarScaler.ChangeScale(Actor.Body.Height.Value,playerAvatar);

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