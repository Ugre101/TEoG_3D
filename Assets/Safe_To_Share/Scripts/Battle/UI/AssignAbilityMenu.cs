using System.Collections;
using System.Collections.Generic;
using Battle;
using Character;
using CustomClasses;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Battle.UI {
    public sealed class AssignAbilityMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu {
        //    [SerializeField] AbilityDict abilityDict;
        [SerializeField] Transform content;
        [SerializeField] AssignAbilityIcon abilityIcon;

        [SerializeField] AssignAbilityIcon[] preInstancedAbilityIcons;

        List<AssignAbilityIcon> abilityIcons = new();

        Queue<AssignAbilityIcon> iconQue;

        ControlledCharacter lastChar;

        public void OnDestroy() {
            StopAllCoroutines();
            AttackBtn.BindActionToMe -= BindButton;
            AssignAbilityIcon.AbilityBound -= Done;
        }

#if UNITY_EDITOR
        void OnValidate() => preInstancedAbilityIcons = content.GetComponentsInChildren<AssignAbilityIcon>(true);
#endif

        public bool BlockIfActive() {
            if (!gameObject.activeSelf) return false;
            gameObject.SetActive(false);
            return true;
        }

        AssignAbilityIcon GetAbilityIcon() => iconQue.Count > 0 ? iconQue.Dequeue() : Instantiate(abilityIcon, content);

        public void FirstSetup() {
            AttackBtn.BindActionToMe += BindButton;
            AssignAbilityIcon.AbilityBound += Done;
            iconQue = new Queue<AssignAbilityIcon>(preInstancedAbilityIcons);
            content.SleepChildren();
        }

        void EnqueueIcons() {
            foreach (Transform child in content)
                if (child.TryGetComponent(out AssignAbilityIcon icon) && icon.gameObject.activeSelf) {
                    iconQue.Enqueue(icon);
                    icon.gameObject.SetActive(false);
                }
        }

        public void Done() => gameObject.SetActive(false);

        void BindButton(AttackBtn obj) {
            if (gameObject.activeSelf || BattleManager.CurrentPlayerControlled == null) {
                Done();
                return;
            }

            gameObject.SetActive(true);
            if (BattleManager.CurrentPlayerControlled.Character is not ControlledCharacter baseCharacter)
                return;
            if (lastChar != null && lastChar.Identity.ID == baseCharacter.Identity.ID) {
                ChangeAbilityIconsButtonBind(obj);
                return;
            }

            EnqueueIcons();
            LoadAbilities(baseCharacter.AndSpellBook.Abilities, obj);
            lastChar = baseCharacter;
        }

        void LoadAbilities(HashSet<string> guids, AttackBtn attackBtn) {
            abilityIcons = new List<AssignAbilityIcon>();
            foreach (var guid in guids)
                StartCoroutine(Load(guid));

            IEnumerator Load(string guid) {
                var op = Addressables.LoadAssetAsync<Ability>(guid);
                yield return op;
                if (op.Task.IsCompleted) {
                    if (op.Status != AsyncOperationStatus.Succeeded)
                        yield break;
                    var icon = GetAbilityIcon();
                    icon.Setup(op.Result, attackBtn);
                    abilityIcons.Add(icon);
                } else if (op.Task.IsFaulted) {
                    Debug.LogWarning("ability loading failed");
                } else if (op.Task.IsCanceled) {
                    Debug.Log("ability loading canceled");
                }
            }
        }

        void ChangeAbilityIconsButtonBind(AttackBtn obj) {
            foreach (var icon in abilityIcons) icon.ChangeButton(obj);
        }
    }
}