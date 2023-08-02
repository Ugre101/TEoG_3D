using System;
using Character;
using UnityEngine;

namespace Dialogue.DialogueActions {
    [Serializable]
    public abstract class DialogueBaseAction  {
        public abstract bool MeetsCondition();
        public abstract void Invoke(BaseCharacter toAdd);
    }
}