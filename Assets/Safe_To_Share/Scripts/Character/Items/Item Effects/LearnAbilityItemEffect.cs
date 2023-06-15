using System;
using Character;
using CustomClasses;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class LearnAbilityItemEffect : ItemEffect {
        [SerializeField] DropSerializableObject<SerializableScriptableObject> dropSerializableObject;

        public override void OnUse(BaseCharacter user, string itemGuid) {
            if (user is ControlledCharacter controlledCharacter)
                controlledCharacter.AndSpellBook.LearnAbility(dropSerializableObject.guid);
        }
    }
}