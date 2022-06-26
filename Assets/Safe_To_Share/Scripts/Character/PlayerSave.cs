using System;
using Items;
using SaveStuff;
using UnityEngine;

namespace Character
{
    [Serializable]
    public struct PlayerSave
    {
        [SerializeField] ControlledCharacterSave controlledCharacterSave;
        [SerializeField] Vector3 posistion;
        [SerializeField] InventorySave inventorySave;

        public PlayerSave(ControlledCharacterSave controlledCharacterSave, Vector3 posistion,
            InventorySave inventorySave)
        {
            this.controlledCharacterSave = controlledCharacterSave;
            this.posistion = posistion;
            this.inventorySave = inventorySave;
        }

        public ControlledCharacterSave ControlledCharacterSave => controlledCharacterSave;

        public Vector3 Posistion => posistion;

        public InventorySave InventorySave => inventorySave;
    }
}