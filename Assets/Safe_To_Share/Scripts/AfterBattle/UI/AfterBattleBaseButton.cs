using Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.UI {
    public abstract class AfterBattleBaseButton : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected Image icon;
        protected bool HasAct;
        protected AfterBattleBaseAction MyAct;
        public bool Empty { get; protected set; } = true;


        void BaseSetup(Sprite newIcon, string newTitle) {
            icon.sprite = newIcon;
            title.text = newTitle;
            Empty = false;
            gameObject.SetActive(true);
        }

        public void Clear() {
            gameObject.SetActive(false);
            //icon.sprite = null;
            //title.text = string.Empty;
            Empty = true;
            if (HasAct)
                MyAct.ClearMe -= Clear;
            MyAct = null;
            HasAct = false;
        }

        public virtual void Setup(AfterBattleBaseAction action) {
            MyAct = action;
            HasAct = true;
            BaseSetup(action.Icon, action.Title);
            action.ClearMe += Clear;
        }

        public bool CanStillDo(BaseCharacter caster, BaseCharacter partner) {
            if (HasAct && MyAct.CanUse(caster, partner))
                return true;
            Clear();
            return false;
        }

        public abstract void Click();
    }
}