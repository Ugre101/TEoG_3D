using System;
using System.Collections.Generic;
using Character.BodyStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.DefeatScenarios.Custom {
    [Serializable]
    public class CustomBodyNode : CustomLoseScenarioNode {
        public BodyStatType bodyType = BodyStatType.Height;
        public List<MakeTempMod> tempMods = new();
        public float permChange;
        public bool transfer;

        public CustomBodyNode(string id, Vector2 canvasPos) : base(id, canvasPos) { }

        public override void HandleEffects(BaseCharacter activeEnemyActor, BaseCharacter activePlayerActor) {
            if (!activePlayerActor.Body.BodyStats.TryGetValue(bodyType, out var playerBody))
                return;
            HandlePermChange(activeEnemyActor, playerBody);
            if (tempMods is { Count: > 0, })
                foreach (var mod in tempMods)
                    playerBody.Mods.AddTempStatMod(new TempIntMod(mod.Duration, mod.Value, "CustomScenario",
                        mod.ModType));
            activeEnemyActor.InvokeUpdateAvatar();
            activePlayerActor.InvokeUpdateAvatar();
        }

        void HandlePermChange(BaseCharacter activeEnemyActor, BodyStat playerBody) {
            if (permChange == 0)
                return;
            if (!transfer || !activeEnemyActor.Body.BodyStats.TryGetValue(bodyType, out var enemyBody)) {
                playerBody.BaseValue += permChange;
                return;
            }

            if (permChange < 0 && playerBody.BaseValue < permChange) {
                enemyBody.BaseValue += playerBody.BaseValue;
                playerBody.BaseValue = 0;
            } else {
                enemyBody.BaseValue -= permChange;
                playerBody.BaseValue += permChange;
            }
        }
    }
}