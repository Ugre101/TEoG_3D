using Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public abstract class AfterBattleBaseButton : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected Image icon;
        protected AfterBattleBaseAction MyAct;
        public bool Empty { get; protected set; } = true;

        void BaseSetup(Sprite newIcon, string newTitle)
        {
            gameObject.SetActive(true);
            icon.sprite = newIcon;
            title.text = newTitle;
            Empty = false;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
            icon.sprite = null;
            title.text = string.Empty;
            Empty = true;
            if (MyAct != null)
                MyAct.ClearMe -= Clear;
            MyAct = null;
        }

        public virtual void Setup(AfterBattleBaseAction action)
        {
            MyAct = action;
            BaseSetup(action.Icon, action.Title);
            action.ClearMe += Clear;
        }

        public bool CanStillDo(BaseCharacter caster, BaseCharacter partner)
        {
            if (MyAct == null || !MyAct.CanUse(caster, partner))
            {
                Clear();
                return false;
            }

            return true;
        }

        public abstract void Click();
    }
}