using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.DefeatScenarios.Nodes
{
    public class LoseScenarioNode : BaseEditorCanvasNode
    {
        [SerializeField] string introText;
        [SerializeField] string resistText;
        [SerializeField] string giveInText;
        [SerializeField, Range(0, 100),] int resistCost;

        public int ResistCost => resistCost;

        public string IntroText => introText;

        public string ResistText => resistText;

        public string GiveInText => giveInText;

        public virtual bool CanDo(BaseCharacter caster, BaseCharacter target) => true;

        public virtual void HandleEffects(BaseCharacter caster,BaseCharacter target)
        {
        }
    }
}