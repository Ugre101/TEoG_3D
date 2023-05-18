using System.Collections;
using System.Threading.Tasks;
using AvatarStuff;
using Battle;
using Character;
using Character.StatsStuff.HealthStuff;
using Safe_To_Share.Scripts.Battle.UI;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.CombatantStuff
{
    public sealed class Combatant : MonoBehaviour
    {
        static readonly int BattleIdle = Animator.StringToHash("Battle");
        static readonly int DeadAnimation = Animator.StringToHash("dead");
        [SerializeField] AvatarInfoDict avatarDict;
        [SerializeField] CharacterFrame characterFrame;
        [SerializeField] GameObject targetFrame;
        [SerializeField] AvatarChanger avatarChanger;
        [SerializeField] bool playerAvatar;
        readonly WaitForSeconds waitForSeconds = new(0.5f);
        Animator activeAnimator;
        public BaseCharacter Character { get; private set; }
        Health Hp => Character?.Stats.Health;
        Health Wp => Character?.Stats.WillPower;

        void OnDestroy()
        {
            UnSub();
        }

        void Sub()
        {
        }

        public void NewAvatar(Animator obj)
        {
            activeAnimator = obj;
            activeAnimator.SetBool(BattleIdle, true);
            if (Character != null)
                BindAvatarReactions();
        }

        void UnSub()
        {
            if (Character == null)
                return;
            Hp.CurrentValueChange -= Dead;
            Wp.CurrentValueChange -= Dead;
            Hp.ValueDecrease -= ReactTakeHealthDamage;
            Wp.ValueDecrease -= ReactTakeWillDamage;
        }

        public async Task Setup(BaseCharacter character)
        {
            UnSub();
            Sub();
            Character = character;
            var loaded = await avatarDict.GetAvatarLoaded(character, playerAvatar);
            avatarChanger.UpdateAvatar(loaded);
            //transform.eulerAngles = Vector3.zero;
            characterFrame.gameObject.SetActive(true);
            characterFrame.Setup(character);
        }

        void ModifyAvatar(CharacterAvatar obj) => obj.Setup(Character);

        void BindAvatarReactions()
        {
            Hp.CurrentValueChange += Dead;
            Wp.CurrentValueChange += Dead;
            Hp.ValueDecrease += ReactTakeHealthDamage;
            Wp.ValueDecrease += ReactTakeWillDamage;
        }

        void ReactTakeWillDamage(int obj) =>
            FloatAnimations(FloatBattleAnimations.TakeWillDamage, (float)obj / Wp.Value);

        void ReactTakeHealthDamage(int obj) =>
            FloatAnimations(FloatBattleAnimations.TakeHealthDamage, (float)obj / Hp.Value);


        void Dead(int obj)
        {
            if (obj <= 0)
                Die();
        }

        void Die()
        {
            activeAnimator.SetBool(DeadAnimation, true);
            characterFrame.gameObject.SetActive(false);
        }

        public void Revive()
        {
            activeAnimator.SetBool(DeadAnimation, false);
            characterFrame.gameObject.SetActive(true);
        }

        public void CastPrefabOn(BattlePrefabEffect onCasterEffect)
        {
            GameObject temp = Instantiate(onCasterEffect.Prefab, transform);
            Destroy(temp, onCasterEffect.StayTime);
        }

        public void Target() => targetFrame.SetActive(true);

        public void StopTargeting() => targetFrame.SetActive(false);

        public void TriggerAnimation(TriggerBattleAnimations battleAnimation)
        {
            if (activeAnimator == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Combatant has no animator");
#endif
                return;
            }

            if (BattleAnimationDict.TriggerAnimations.TryGetValue(battleAnimation, out int hash))
                activeAnimator.SetTrigger(hash);
        }

        public void FloatAnimations(FloatBattleAnimations animations, float value)
        {
            if (activeAnimator == null)
            {
                Debug.LogWarning("Combatant has no animator");
                return;
            }

            if (!BattleAnimationDict.FloatAnimations.TryGetValue(animations, out int hast)) return;
            activeAnimator.SetFloat(hast, value);
            StartCoroutine(ResetFloat(hast));
        }

        IEnumerator ResetFloat(int hash)
        {
            yield return waitForSeconds;
            activeAnimator.SetFloat(hash, 0);
        }
    }
}