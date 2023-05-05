using Character.DefeatScenarios.Nodes;
using CustomClasses;
using UnityEngine;

namespace Defeated
{
    [CreateAssetMenu(fileName = "New Lose Scenario", menuName = "Defeat/Lose Scenario", order = 0)]
    public class LoseScenario : BaseEditorCanvasObject<LoseScenarioNode>
    {
        [SerializeField] string title;
    }
}