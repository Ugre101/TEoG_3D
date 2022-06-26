using System;
using UnityEngine;

namespace Character.DefeatScenarios.Custom
{
    [Serializable]
    public class CustomIntroNode : CustomLoseScenarioNode
    {
        public CustomIntroNode(Vector2 canvasPos) : base(Guid.NewGuid().ToString(), canvasPos)
        {
        }
    }
}