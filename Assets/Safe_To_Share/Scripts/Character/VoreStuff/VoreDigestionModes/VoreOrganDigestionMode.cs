using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.VoreStuff.VoreDigestionModes
{
    [Serializable]
    public abstract class VoreOrganDigestionMode
    {
        public const string Endo = "Endosoma", Digestion = "Digestion", Absorption = "Absorption";
        [SerializeField] private int currentMode;
        protected DigestionMethod digestionMethod;
     
        public DigestionMethod DigestionMethod
        {
            get
            {
                if (digestionMethod == null)
                    SetDigestionMode(CurrentModeID);
                return digestionMethod;
            }
        }
        public abstract string[] AllDigestionTypes { get; }
        public virtual IEnumerable<string> GetPossibleDigestionTypes(BaseCharacter pred)
        {
            yield return Endo;
            yield return Digestion;
        }
        public abstract void SetDigestionMode (int type);
        public string CurrentModeTitle
        {
            get
            {
                if (AllDigestionTypes == null || CurrentModeID >= AllDigestionTypes.Length)
                    return "error";
                return AllDigestionTypes[CurrentModeID];
            }
        }

        public int CurrentModeID { get => currentMode; protected set => currentMode = value; }

        public bool IsEndo() => CurrentModeTitle == Endo;
    }
}