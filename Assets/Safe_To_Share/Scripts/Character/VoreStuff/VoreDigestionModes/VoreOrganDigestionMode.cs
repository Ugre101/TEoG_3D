using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes
{
    [Serializable]
    public abstract class VoreOrganDigestionMode
    {
        public const string Endo = "Endosoma", Digestion = "Digestion", Absorption = "Absorption";
        [SerializeField] int currentMode;
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

        public string CurrentModeTitle
        {
            get
            {
                if (AllDigestionTypes == null || CurrentModeID >= AllDigestionTypes.Length)
                    return "error";
                return AllDigestionTypes[CurrentModeID];
            }
        }

        public int CurrentModeID
        {
            get => currentMode;
            protected set => currentMode = value;
        }

        public virtual IEnumerable<string> GetPossibleDigestionTypes(BaseCharacter pred)
        {
            yield return Endo;
            yield return Digestion;
        }

        public abstract void SetDigestionMode(int type);

        public bool IsEndo() => CurrentModeTitle == Endo;
    }
}