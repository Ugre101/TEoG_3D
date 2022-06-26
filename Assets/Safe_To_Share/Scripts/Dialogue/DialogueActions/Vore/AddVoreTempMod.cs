using System;
using System.Collections.Generic;
using Character;
using Character.StatsStuff.Mods;
using Character.VoreStuff;
using UnityEngine;

namespace Dialogue.DialogueActions.Vore
{
    [Serializable]
    public class AddVoreTempMod : DialogueVoreAction
    {
        [SerializeField] List<TempIntMod> mods = new();
        [SerializeField] bool pleasure;
        public override bool MeetsCondition() => true;

        public override void Invoke(BaseCharacter toAdd, Prey prey, VoreOrgan container)
        {
            foreach (TempIntMod tempIntMod in mods)
            {
                if (pleasure)
                    toAdd.Vore.pleasureDigestion.Mods.AddTempStatMod(tempIntMod);
                else
                    toAdd.Vore.digestionStrength.Mods.AddTempStatMod(tempIntMod);
            }
            
        }
    }
}