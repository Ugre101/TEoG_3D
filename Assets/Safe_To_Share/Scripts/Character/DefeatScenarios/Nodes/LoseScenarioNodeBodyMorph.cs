using System.Collections.Generic;
using Character.BodyStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.DefeatScenarios.Nodes {
    public sealed class LoseScenarioNodeBodyMorph : LoseScenarioNode {
        [SerializeField] bool transferToSelf;
        [SerializeField] BodyStatType bodyType;
        [SerializeField, Range(-25, 25),] int permChange;
        [SerializeField] List<TempIntMod> tempChange = new();

        public override void HandleEffects(BaseCharacter caster, BaseCharacter target) {
            if (target.Body.BodyStats.TryGetValue(bodyType, out var bodyStat))
                HandleBodyEffects(caster, bodyStat);
            caster.InvokeUpdateAvatar();
            //caster.UpdateHeight(); TODO Check if still works
            target.InvokeUpdateAvatar();
            //target.UpdateHeight();
            base.HandleEffects(caster, target);
        }

        void HandleBodyEffects(BaseCharacter caster, BodyStat bodyStat) {
            foreach (var tempIntMod in tempChange)
                bodyStat.Mods.AddTempStatMod(tempIntMod);
            if (permChange == 0) return;
            if (bodyStat.BaseValue + permChange > 1) {
                bodyStat.BaseValue += permChange;
                if (transferToSelf && caster.Body.BodyStats.TryGetValue(bodyType, out var casterBody))
                    casterBody.BaseValue -= permChange;
            } else {
                var left = bodyStat.BaseValue - 1;
                if (left <= 0) return;
                bodyStat.BaseValue -= left;
                if (transferToSelf && caster.Body.BodyStats.TryGetValue(bodyType, out var casterBody))
                    casterBody.BaseValue += left;
            }
        }
    }
}