using System;
using Character;
using Character.BodyStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects
{
    [Serializable]
    public class GrowBody : Effect
    {
        [SerializeField] BodyStatType bodyType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            if (target.Body.BodyStats.TryGetValue(bodyType, out var body))
                body.BaseValue += FinalValue(user, body.BaseValue);
        }
    }
}