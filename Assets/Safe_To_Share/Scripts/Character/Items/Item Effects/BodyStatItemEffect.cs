using System;
using System.Collections.Generic;
using Character;
using Character.BodyStuff;
using Character.StatsStuff.Mods;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects
{
    [Serializable]
    public class BodyStatItemEffect : ItemEffect
    {
        [SerializeField] List<AssignBodyTempMod> bodyTempMods = new();
        [SerializeField] List<BodyChange> bodyChanges = new();

        [Header("Thickset"), SerializeField, Range(-1f, 1f),]
        float thickSetChange;

        [SerializeField] List<TempIntMod> tempThickMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (AssignBodyTempMod assignBodyTempMod in bodyTempMods)
                if (user.Body.BodyStats.TryGetValue(assignBodyTempMod.bodyStatType, out BodyStat bodyStat))
                    assignBodyTempMod.assignTempMod.AddMods(bodyStat.Mods, itemGuid);
            foreach (BodyChange bodyChange in bodyChanges)
                if (user.Body.BodyStats.TryGetValue(bodyChange.bodyStatType, out BodyStat bodyStat))
                    bodyStat.BaseValue += bodyChange.permChange;
            if (thickSetChange != 0)
                user.Body.Thickset.BaseValue += thickSetChange;
            foreach (TempIntMod tempThickMod in tempThickMods)
                user.Body.Thickset.Mods.AddTempStatMod(tempThickMod);
        }

        [Serializable]
        struct AssignBodyTempMod
        {
            public BodyStatType bodyStatType;
            public AssignTempMod assignTempMod;
        }

        [Serializable]
        struct BodyChange
        {
            public BodyStatType bodyStatType;
            public float permChange;
        }
    }
}