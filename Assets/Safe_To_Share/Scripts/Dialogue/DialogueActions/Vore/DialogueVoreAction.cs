using System;
using Character;
using Character.VoreStuff;

namespace Dialogue.DialogueActions {
    [Serializable]
    public abstract class DialogueVoreAction {
        public abstract bool MeetsCondition();
        public abstract void Invoke(BaseCharacter pred, Prey prey, VoreOrgan container);
    }
}