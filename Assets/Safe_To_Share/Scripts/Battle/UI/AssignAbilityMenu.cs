using System.Collections.Generic;
using Battle.SkillsAndSpells;
using Character;
using Character.SkillsAndSpells;
using CustomClasses;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Battle.UI
{
    public class AssignAbilityMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu
    {
        //    [SerializeField] AbilityDict abilityDict;
        [SerializeField] Transform content;
        [SerializeField] AssignAbilityIcon abilityIcon;

        [SerializeField] AssignAbilityIcon[] preInstancedAbilityIcons;

#if UNITY_EDITOR
        void OnValidate() => preInstancedAbilityIcons = content.GetComponentsInChildren<AssignAbilityIcon>(true);
#endif

        Queue<AssignAbilityIcon> iconQue;
        AssignAbilityIcon GetAbilityIcon() => iconQue.Count > 0 ? iconQue.Dequeue() : Instantiate(abilityIcon, content);

        ControlledCharacter lastChar = null;
        public void OnDestroy()
        {
            StopAllCoroutines();
            AttackBtn.BindActionToMe -= BindButton;
            AssignAbilityIcon.AbilityBound -= Done;
        }

        public bool BlockIfActive()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }

        public void FirstSetup()
        {
            AttackBtn.BindActionToMe += BindButton;
            AssignAbilityIcon.AbilityBound += Done;
            iconQue = new Queue<AssignAbilityIcon>(preInstancedAbilityIcons);
            content.SleepChildren();
        }

        private void EnqueueIcons()
        {
            foreach (Transform child in content)
                if (child.TryGetComponent(out AssignAbilityIcon icon) && icon.gameObject.activeSelf)
                {
                    iconQue.Enqueue(icon);
                    icon.gameObject.SetActive(false);
                }
        }

        public void Done() => gameObject.SetActive(false);

        void BindButton(AttackBtn obj)
        {
            if (gameObject.activeSelf || BattleManager.CurrentPlayerControlled == null)
            {
                Done();
                return;
            }

            gameObject.SetActive(true);
            if (BattleManager.CurrentPlayerControlled.Character is not ControlledCharacter baseCharacter) 
                return;
            if (lastChar != null && lastChar.Identity.ID == baseCharacter.Identity.ID)
            {
                ChangeAbilityIconsButtonBind(obj);
                return;
            }
            EnqueueIcons();
            Test(baseCharacter.AndSpellBook.Abilities,obj);
            lastChar = baseCharacter;
        }

        List<AssignAbilityIcon> abilityIcons = new();
        void Test(HashSet<string> guids, AttackBtn attackBtn)
        {
            abilityIcons = new List<AssignAbilityIcon>();
            foreach (string guid in guids) 
                Load(guid);

            async void Load(string guid)
            {
                var op = Addressables.LoadAssetAsync<Ability>(guid);
                await op.Task;
                if (op.Task.IsCompleted)
                {
                    if (op.Status != AsyncOperationStatus.Succeeded) return;
                    var icon = GetAbilityIcon();
                    icon.Setup(op.Result, attackBtn);
                    abilityIcons.Add(icon);
                }
                else if (op.Task.IsFaulted) Debug.LogWarning("ability loading failed");
                else if (op.Task.IsCanceled) Debug.Log("ability loading canceled");
            }
        }

        private void ChangeAbilityIconsButtonBind(AttackBtn obj)
        {
            foreach(var icon in abilityIcons)
            {
                icon.ChangeButton(obj);
            }
        }

    }
}