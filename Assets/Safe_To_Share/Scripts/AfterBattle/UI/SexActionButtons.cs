using System.Collections.Generic;
using System.Linq;
using Character;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.UI {
    public sealed class SexActionButtons : MonoBehaviour {
        [SerializeField] SexActionsDict dict;
        [SerializeField] TakeToDormButton takeToDorm;
        [SerializeField] DrainActionButton[] drainButtons;
        [SerializeField] SexActionButton[] sexActButtons;
        [SerializeField] VoreActionButton[] voreActButtons;
        List<EssenceAction> addedDrainActs = new();
        List<SexAction> addedSexActs = new();
        List<VoreAction> addedVoreActs = new();

#if UNITY_EDITOR
        void OnValidate() {
            sexActButtons = GetComponentsInChildren<SexActionButton>(true);
            drainButtons = GetComponentsInChildren<DrainActionButton>(true);
            voreActButtons = GetComponentsInChildren<VoreActionButton>(true);
        }
#endif

        public void FirstSetup(BaseCharacter buttonsOwner, BaseCharacter partner) {
            foreach (var sexActionButton in sexActButtons)
                sexActionButton.Clear();
            foreach (var drainBtn in drainButtons)
                drainBtn.Clear();
            foreach (var voreBtn in voreActButtons)
                voreBtn.Clear();
            Refresh(buttonsOwner, partner);
            if (OptionalContent.Vore.Enabled) return;
            foreach (var voreActionButton in voreActButtons)
                voreActionButton.Clear();
        }

        public void Refresh(BaseCharacter buttonOwner, BaseCharacter partner) {
            // Add new sex actions
            addedSexActs = FirstStep(addedSexActs, buttonOwner, partner);
            var sexActions = dict.SexActsWeCanDo(buttonOwner, partner).Except(addedSexActs);
            SecondStep(sexActions, sexActButtons, ref addedSexActs);

            // Add new drain actions
            addedDrainActs = FirstStep(addedDrainActs, buttonOwner, partner);
            var toAddDrainAct = dict.DrainActionsWeCanDo(buttonOwner, partner).Except(addedDrainActs);
            SecondStep(toAddDrainAct, drainButtons, ref addedDrainActs);

            if (OptionalContent.Vore.Enabled)
                SetupVoreButtons(buttonOwner, partner);
            takeToDorm.ShowOrgasmsLeft(partner);
        }

        static void SecondStep<TAct, TBtn>(IEnumerable<TAct> acts, IReadOnlyList<TBtn> emptyButtons,
                                           ref List<TAct> addedActs)
            where TAct : AfterBattleBaseAction where TBtn : AfterBattleBaseButton {
            var toAddAction = acts.ToArray(); // afterBattleBaseActions.ToArray();
            var buttons = emptyButtons.Where(b => b.Empty).ToArray();

            for (var i = 0; i < buttons.Length && i < toAddAction.Length; i++) {
                buttons[i].Setup(toAddAction[i]);
                addedActs.Add(toAddAction[i]);
            }
        }

        static List<T> FirstStep<T>(List<T> list, BaseCharacter buttonOwner,
                                    BaseCharacter partner) where T : AfterBattleBaseAction {
            var actsWeCantDoAnymore = list.FindAll(a => !a.CanUse(buttonOwner, partner));
            foreach (var afterBattleBaseAction in actsWeCantDoAnymore)
                afterBattleBaseAction.OnClearMe();
            list = list.Except(actsWeCantDoAnymore).ToList();
            return list;
        }

        void SetupVoreButtons(BaseCharacter buttonOwner, BaseCharacter partner) {
            addedVoreActs = FirstStep(addedVoreActs, buttonOwner, partner);

            var voreActions = dict.VoreActionsWeCanDo(buttonOwner, partner).Except(addedVoreActs);
            SecondStep(voreActions, voreActButtons, ref addedVoreActs);

            var cantVoreActs = dict.VoreActions.Except(dict.VoreActionsWeCanDo(buttonOwner, partner)).ToArray();
            var emptyVoreButtons = voreActButtons.Where(b => b.Empty).ToArray();
            for (var i = 0; i < cantVoreActs.Length && i < emptyVoreButtons.Length; i++) {
                if (cantVoreActs[i].NeedPerk)
                    continue;
                var extraCapacityNeeded = cantVoreActs[i].ExtraCapacityNeeded(buttonOwner, partner);
                if (extraCapacityNeeded > 0)
                    emptyVoreButtons[i].ShowNeeded(cantVoreActs[i], extraCapacityNeeded);
            }
        }

        public void Clear(BaseCharacter buttonOwner) {
            foreach (var drainActionButton in drainButtons)
                drainActionButton.Clear();
            foreach (var sexActionButton in sexActButtons)
                sexActionButton.Clear();
            foreach (var voreActionButton in voreActButtons)
                voreActionButton.Clear();
            takeToDorm.gameObject.SetActive(false);
        }
    }
}