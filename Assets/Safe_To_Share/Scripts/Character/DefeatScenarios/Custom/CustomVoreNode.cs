using System;
using System.Collections.Generic;
using System.Linq;
using Character.VoreStuff;
using UnityEngine;
using Random = System.Random;

namespace Character.DefeatScenarios.Custom
{
    [Serializable]
    public class CustomVoreNode : CustomLoseScenarioNode
    {
        public List<VoreType> voreTypes = new();

        public CustomVoreNode(string id, Vector2 canvasPos) : base(id, canvasPos)
        {
        }

        public override bool CanDo(BaseCharacter enemy, BaseCharacter player)
        {
            if (voreTypes == null || voreTypes.Count == 0)
                return false;
            return voreTypes.Any(type => VoreSystemExtension.CanDoOfType(enemy, player, type));
        }

        public override void HandleEffects(BaseCharacter activeEnemyActor, BaseCharacter activePlayerActor)
        {
            List<VoreType> possible = voreTypes.Where(type =>
                VoreSystemExtension.CanDoOfType(activeEnemyActor, activePlayerActor, type)).ToList();
            if (!activeEnemyActor.VoreOfType(activePlayerActor, possible[new Random().Next(possible.Count)]))
                return;
            activePlayerActor.InvokeRemoveAvatar();
            activeEnemyActor.InvokeUpdateAvatar();
        }
    }
}