using System;
using System.Collections;
using Character.CreateCharacterStuff;
using Character.SkillsAndSpells;
using SaveStuff;
using UnityEngine;

namespace Character {
    [Serializable]
    public class ControlledCharacter : BaseCharacter {
        [SerializeField] AbilityBook abilityBook = new();

        public ControlledCharacter(ControlledCharacter character) : base(character) =>
            abilityBook = character.AndSpellBook;

        protected ControlledCharacter() { }

        public ControlledCharacter(CreateCharacter character) : base(character) {
            foreach (var startAbility in character.StartAbilities)
                AndSpellBook.LearnAbility(startAbility);
        }

        public AbilityBook AndSpellBook => abilityBook;

        public IEnumerator Load(ControlledCharacterSave toLoad) {
            yield return base.Load(toLoad.CharacterSave);
            AndSpellBook.Load(toLoad.AbilitySave);
        }
    }
}