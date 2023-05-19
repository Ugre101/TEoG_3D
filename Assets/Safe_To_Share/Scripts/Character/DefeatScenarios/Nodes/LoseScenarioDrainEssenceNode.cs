using System;
using Character.EssenceStuff;
using UnityEngine;

namespace Character.DefeatScenarios.Nodes
{
    public sealed class LoseScenarioDrainEssenceNode : LoseScenarioNode
    {
        [SerializeField] DrainEssenceType drainEssenceType = DrainEssenceType.Both;

        public override bool CanDo(BaseCharacter caster, BaseCharacter target) =>
            drainEssenceType switch
            {
                DrainEssenceType.None => true,
                DrainEssenceType.Masc => target.CanDrainMasc(),
                DrainEssenceType.Femi => target.CanDrainFemi(),
                DrainEssenceType.Both => target.CanDrainMasc() || target.CanDrainFemi(),
                _ => throw new ArgumentOutOfRangeException(),
            };

        public override void HandleEffects(BaseCharacter caster, BaseCharacter target)
        {
            caster.DrainEssenceOfType(target, drainEssenceType);
            caster.InvokeUpdateAvatar();
            target.InvokeUpdateAvatar();
        }
    }
}