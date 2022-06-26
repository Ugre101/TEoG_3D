using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.DefeatScenarios.Custom
{
    [Serializable]
    public abstract class CustomLoseScenarioNode
    {
        public string id;
        public string introText;
        public string resistText;
        public string giveInText;
        public int resistCost;
        public Vector2 canvasPos;
        public List<string> childNodesIds = new();

        protected CustomLoseScenarioNode(string id, Vector2 canvasPos)
        {
            this.id = id;
            this.canvasPos = canvasPos;
        }

        public virtual bool CanDo(BaseCharacter enemy, BaseCharacter player) => true;

        public virtual void HandleEffects(BaseCharacter activeEnemyActor, BaseCharacter activePlayerActor)
        {
        }
    }
}