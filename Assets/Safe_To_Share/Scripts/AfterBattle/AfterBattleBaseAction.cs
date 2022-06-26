using System;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public abstract class AfterBattleBaseAction : ScriptableObject
    {
        [SerializeField] Sprite icon;
        [SerializeField] string title;
        [SerializeField] protected SexActData data;
        [SerializeField] protected SexActData errorData;
        public Sprite Icon => icon;
        public string Title => title;
        public abstract bool CanUse(BaseCharacter giver, BaseCharacter receiver);
        public abstract SexActData Use(AfterBattleActor caster, AfterBattleActor target);

        public event Action ClearMe;

        public virtual void OnClearMe() => ClearMe?.Invoke();
    }
}