using System;
using Character.EssenceStuff;
using UnityEngine;

namespace Character.DefeatScenarios.Custom {
    [Serializable]
    public class CustomDrainNode : CustomLoseScenarioNode {
        public DrainEssenceType drainEssenceType = DrainEssenceType.Both;
        public DrainEssenceType giveEssenceType = DrainEssenceType.Both;
        public int DrainBonus;
        public int GiveBonus;

        public CustomDrainNode(string id, Vector2 canvasPos) : base(id, canvasPos) { }

        public override void HandleEffects(BaseCharacter activeEnemyActor, BaseCharacter activePlayerActor) {
            HandleDrain(activeEnemyActor, activePlayerActor);
            HandleGive(activeEnemyActor, activePlayerActor);
            activeEnemyActor.InvokeUpdateAvatar();
            activePlayerActor.InvokeUpdateAvatar();
        }

        void HandleGive(BaseCharacter activeEnemyActor, BaseCharacter activePlayerActor) {
            if (GiveBonus <= 0)
                return;
            switch (giveEssenceType) {
                case DrainEssenceType.None:
                    break;
                case DrainEssenceType.Masc:
                    activePlayerActor.GainMasc(GiveBonus);
                    break;
                case DrainEssenceType.Femi:
                    activePlayerActor.GainFemi(GiveBonus);
                    break;
                case DrainEssenceType.Both:
                    activePlayerActor.GainMasc(GiveBonus / 2);
                    activePlayerActor.GainFemi(GiveBonus / 2);
                    break;
            }
        }

        void HandleDrain(BaseCharacter activeEnemyActor, BaseCharacter activePlayerActor) =>
            activeEnemyActor.DrainEssenceOfType(activePlayerActor, drainEssenceType, DrainBonus);
    }
}