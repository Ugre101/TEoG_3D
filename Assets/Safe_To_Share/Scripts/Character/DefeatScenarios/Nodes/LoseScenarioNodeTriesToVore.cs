using System.Linq;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.DefeatScenarios.Nodes
{
    public class LoseScenarioNodeTriesToVore : LoseScenarioNode
    {
        [SerializeField] VoreType[] voreTypes;

        public override bool CanDo(BaseCharacter caster, BaseCharacter target) => OptionalContent.Vore.Enabled &&
                                                                                  voreTypes.Any(voreType =>
                                                                                      VoreSystemExtension.CanDoOfType(
                                                                                          caster, target, voreType));

        public override void HandleEffects(BaseCharacter caster, BaseCharacter target)
        {
            foreach (VoreType voreType in voreTypes)
                if (VoreSystemExtension.CanDoOfType(caster, target, voreType) && caster.VoreOfType(target, voreType))
                {
                    target.InvokeRemoveAvatar();
                    caster.InvokeUpdateAvatar();
                }
        }
    }
}