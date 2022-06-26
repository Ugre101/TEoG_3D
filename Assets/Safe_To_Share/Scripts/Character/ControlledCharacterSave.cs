using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;

namespace SaveStuff
{
    [Serializable]
    public struct ControlledCharacterSave
    {
        [SerializeField] CharacterSave characterSave;
        [SerializeField] List<string> abilitySave;

        public ControlledCharacterSave(ControlledCharacter character)
        {
            characterSave = new CharacterSave(character);
            abilitySave = character.AndSpellBook.Abilities.ToList();
        }

        public List<string> AbilitySave => abilitySave;
        public CharacterSave CharacterSave => characterSave;
    }
}