using System;
using System.Collections;
using Battle.SkillsAndSpells;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Battle
{
    [RequireComponent(typeof(Button))]
    public sealed class AttackBtn : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] InputActionReference reference;
        [SerializeField] TextMeshProUGUI numberText;
        [SerializeField] Image icon;
        [SerializeField] Button btn;
        Ability ability;
        int id;

        void OnDestroy()
        {
            reference.action.performed -= ClickButton;
            reference.action.Disable();
            StopAllCoroutines();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                BindNewAbility();
        }

        public static event Action<Ability> PlayerAction;
        public static event Action<AttackBtn> BindActionToMe;
        public static event Action<int, Ability> BoundAbility;

        public void ChangedDevice(PlayerInput inputs) => UpdateRefText();

        public void ClickButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                UseAbility();
        }

        public void AltClickButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                BindNewAbility();
        }

        public void FirstSetup()
        {
            icon.gameObject.SetActive(false);
            btn.onClick.AddListener(UseAbility);
            reference.action.performed += ClickButton;
            reference.action.Enable();
            UpdateRefText();
        }

        public void SetId(int buttonNumber) => id = buttonNumber;
        void UpdateRefText() => numberText.text = reference.action.GetBindingDisplayString();

        public void BindAbility(string newAbility) => StartCoroutine(DoesThisWork(newAbility));

        IEnumerator DoesThisWork(string newAbility)
        {
            var obj = Addressables.LoadResourceLocationsAsync(newAbility);
            yield return obj;
            if (obj.Status is not AsyncOperationStatus.Succeeded || obj.Result.Count == 0)
            {
                Addressables.Release(obj);
                Clear();
                yield break;
            }

            var ab = Addressables.LoadAssetAsync<Ability>(obj.Result[0]);
            yield return ab;
            ability = ab.Result;
            icon.gameObject.SetActive(true);
            icon.sprite = ability.Icon;
        }

        public void BindNewAbility(Ability newAbility)
        {
            ability = newAbility;
            icon.gameObject.SetActive(true);
            icon.sprite = ability.Icon;
            BoundAbility?.Invoke(id, newAbility);
        }

        public void Clear()
        {
            ability = null;
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        }

        void BindNewAbility() => BindActionToMe?.Invoke(this);

        void UseAbility()
        {
            if (ability == null)
                BindNewAbility();
            else
                PlayerAction?.Invoke(ability);
        }
    }
}