using Character;
using Character.EssenceStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle {
    public abstract class EssenceAction : AfterBattleBaseAction {
        [SerializeField] string desc;
        [SerializeField] bool drainerOrgasmNeeded, victimOrgasmNeeded;
        [SerializeField] int orgasmsNeeded = 1;
        [SerializeField] protected SexActData returnData;
        [SerializeField] protected GameObject effectPrefab;

        public int OrgasmsNeeded => orgasmsNeeded;

        public override bool CanUse(BaseCharacter drainer, BaseCharacter victim) {
            if (drainerOrgasmNeeded && !drainer.SexStats.HasEnoughOrgasms(orgasmsNeeded))
                return false;
            if (victimOrgasmNeeded && !victim.SexStats.HasEnoughOrgasms(orgasmsNeeded))
                return false;
            return true;
        }

        protected void ShowEffect(Transform here) {
            if (effectPrefab == null) return;
            var effect = Instantiate(effectPrefab, here);
            Destroy(effect, 3f);
        }

        protected SexActData SexActData(ChangeLog drainLog) {
            var actData = returnData;
            //  actData.AfterText.AddRange(drainLog.DrainLogs);
            //  actData.AfterText.AddRange(drainLog.GainLogs);
            return actData;
        }
    }
}