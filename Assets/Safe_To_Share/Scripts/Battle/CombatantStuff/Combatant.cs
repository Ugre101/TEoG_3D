using System;
using System.Collections;
using System.Threading.Tasks;
using AvatarStuff;
using Battle.UI;
using Character;
using Character.StatsStuff.HealthStuff;
using UnityEngine;

namespace Battle.CombatantStuff
{
    public class Combatant : MonoBehaviour
    {
        [SerializeField] AvatarInfoDict avatarDict;
        [SerializeField] CharacterFrame characterFrame;
        [SerializeField] GameObject targetFrame;
        [SerializeField] AvatarChanger avatarChanger;
        [SerializeField] bool playerAvatar;
        Animator activeAnimator;
        static readonly int BattleIdle = Animator.StringToHash("Battle");
        static readonly int DeadAnimation = Animator.StringToHash("dead");
        readonly WaitForSeconds waitForSeconds = new(0.5f);
        public BaseCharacter Character { get; private set; }
        Health Hp => Character?.Stats.Health;
        Health Wp => Character?.Stats.WillPower;

        private void Sub()
        {
        }

        public void NewAvatar(Animator obj)
        {
            activeAnimator = obj;
            activeAnimator.SetBool(BattleIdle, true);
            if (Character != null)
                BindAvatarReactions();
        }

        void OnDestroy()
        {
            if (activeAnimator != null)
                activeAnimator.SetBool(BattleIdle, false);
            UnSub();
        }

        private void UnSub()
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

        private void ModifyAvatar(CharacterAvatar obj) => obj.Setup(Character);

        void BindAvatarReactions()
        {
            Hp.CurrentValueChange += Dead;
            Wp.CurrentValueChange += Dead;
            Hp.ValueDecrease += ReactTakeHealthDamage;
            Wp.ValueDecrease += ReactTakeWillDamage;
        }

        void ReactTakeWillDamage(int obj) => 
            FloatAnimations(FloatBattleAnimations.TakeWillDamage,(float)obj/Wp.Value);

        void ReactTakeHealthDamage(int obj) =>
            FloatAnimations(FloatBattleAnimations.TakeHealthDamage, (float) obj / Hp.Value);


        void Dead(int obj)
        {
            if (obj <= 0)
                Die();
        }

        void Die() 
        {
            if (activeAnimator != null)
                activeAnimator.SetBool(DeadAnimation,true);
            if (characterFrame != null)
                characterFrame.gameObject.SetActive(false);
        }

        public void Revive()
        {
            activeAnimator.SetBool(DeadAnimation,false);
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

            if (BattleAnimationDict.FloatAnimations.TryGetValue(animations, out int hast))
            {
                activeAnimator.SetFloat(hast, value);
                StartCoroutine(ResetFloat(hast));
            }
        }

        IEnumerator ResetFloat(int hash)
        {
            yield return waitForSeconds;
            activeAnimator.SetFloat(hash, 0);
        }
    }
}