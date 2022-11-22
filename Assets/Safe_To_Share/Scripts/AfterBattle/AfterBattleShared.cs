﻿using Character;
using Character.PlayerStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public abstract class AfterBattleShared : MonoBehaviour
    {
        [SerializeField] protected AfterBattleActor activePlayerActor;
        [SerializeField] protected AfterBattleActor activeEnemyActor;
        [SerializeField] protected RuntimeAnimatorController animatorController;
        public abstract void Setup(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies);
    }
}