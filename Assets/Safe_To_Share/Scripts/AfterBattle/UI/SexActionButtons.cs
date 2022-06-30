using System.Collections.Generic;
using System.Linq;
using Character;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public class SexActionButtons : MonoBehaviour
    {
        [SerializeField] SexActionsDict dict;
        [SerializeField] TakeToDormButton takeToDorm;
        [SerializeField] DrainActionButton[] drainButtons;
        [SerializeField] SexActionButton[] sexActButtons;
        [SerializeField] VoreActionButton[] voreActButtons;
        List<AfterBattleBaseAction> addedDrainActs = new();
        List<AfterBattleBaseAction> addedSexActs = new();
        List<AfterBattleBaseAction> addedVoreActs = new();

#if UNITY_EDITOR
        void OnValidate()
        {
            sexActButtons = GetComponentsInChildren<SexActionButton>();
            drainButtons = GetComponentsInChildren<DrainActionButton>();
            voreActButtons = GetComponentsInChildren<VoreActionButton>();
        }
#endif

        public void FirstSetup(BaseCharacter buttonsOwner, BaseCharacter partner)
        {
            foreach (SexActionButton actBtn in sexActButtons)
                actBtn.Clear();
            foreach (DrainActionButton drainBtn in drainButtons)
                drainBtn.Clear();
            foreach (VoreActionButton voreBtn in voreActButtons)
                voreBtn.Clear();
            Refresh(buttonsOwner, partner);
            if (!OptionalContent.Vore.Enabled)
                foreach (VoreActionButton voreActionButton in voreActButtons)
                    voreActionButton.Clear();
        }

        public void Refresh(BaseCharacter buttonOwner, BaseCharacter partner)
        {
            // Remove actions we can't do anymore
            addedSexActs = FirstStep(addedSexActs, buttonOwner, partner);
            addedDrainActs = FirstStep(addedDrainActs, buttonOwner, partner);
            addedVoreActs = FirstStep(addedVoreActs, buttonOwner, partner);
            // Add new sex actions
            AfterBattleBaseAction[] toAddAction =
                dict.SexActsWeCanDo(buttonOwner, partner).Except(addedSexActs).ToArray();
            SexActionButton[] emptyButtons = sexActButtons.Where(b => b.Empty).ToArray();
            for (int i = 0; i < emptyButtons.Length && i < toAddAction.Length; i++)
            {
                emptyButtons[i].Setup(toAddAction[i]);
                addedSexActs.Add(toAddAction[i]);
            }

            // Add new drain actions
            AfterBattleBaseAction[] toAddDrainAct =
                dict.DrainActionsWeCanDo(buttonOwner, partner).Except(addedDrainActs).ToArray();
            DrainActionButton[] emptyDrainButtons = drainButtons.Where(b => b.Empty).ToArray();
            for (int i = 0; i < emptyDrainButtons.Length && i < toAddDrainAct.Length; i++)
            {
                emptyDrainButtons[i].Setup(toAddDrainAct[i]);
                addedDrainActs.Add(toAddDrainAct[i]);
            }

            if (OptionalContent.Vore.Enabled)
                SetupVoreButtons(buttonOwner, partner);
            takeToDorm.ShowOrgasmsLeft(partner);
        }

        static List<AfterBattleBaseAction> FirstStep(List<AfterBattleBaseAction> list, BaseCharacter buttonOwner, BaseCharacter partner)
        {
            List<AfterBattleBaseAction> actsWeCantDoAnymore = list.FindAll(a => !a.CanUse(buttonOwner, partner));
            foreach (AfterBattleBaseAction afterBattleBaseAction in actsWeCantDoAnymore)
                afterBattleBaseAction.OnClearMe();
            list = list.Except(actsWeCantDoAnymore).ToList();
            return list;
        }

        void SetupVoreButtons(BaseCharacter buttonOwner, BaseCharacter partner)
        {
            // Add new vore actions
            var toAddVoreAct =
                dict.VoreActionsWeCanDo(buttonOwner, partner).Except(addedVoreActs).ToList();
            var emptyVoreButtons = voreActButtons.Where(b => b.Empty).ToArray();
            for (int i = 0; i < emptyVoreButtons.Length && i < toAddVoreAct.Count; i++)
            {
                emptyVoreButtons[i].Setup(toAddVoreAct[i]);
                addedVoreActs.Add(toAddVoreAct[i]);
            }

            var cantVoreActs = dict.VoreActions.Except(dict.VoreActionsWeCanDo(buttonOwner, partner)).ToList();
            emptyVoreButtons = voreActButtons.Where(b => b.Empty).ToArray();
            for (int i = 0; i < cantVoreActs.Count && i < emptyVoreButtons.Length; i++)
            {
                if (cantVoreActs[i].NeedPerk)
                    continue;
                float extraCapacityNeeded = cantVoreActs[i].ExtraCapacityNeeded(buttonOwner, partner);
                if (extraCapacityNeeded > 0)
                    emptyVoreButtons[i].ShowNeeded(cantVoreActs[i], extraCapacityNeeded);
            }
        }

        public void Refresh(BaseCharacter buttonOwner)
        {
            foreach (DrainActionButton drainActionButton in drainButtons)
                drainActionButton.Clear();
            foreach (SexActionButton sexActionButton in sexActButtons)
                sexActionButton.Clear();
            foreach (VoreActionButton voreActionButton in voreActButtons)
                voreActionButton.Clear();
            takeToDorm.gameObject.SetActive(false);
        }
    }
}