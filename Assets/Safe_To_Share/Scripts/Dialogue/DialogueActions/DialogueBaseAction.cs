using System;
using Character;

namespace Dialogue.DialogueActions
{
    [Serializable]
    public abstract class DialogueBaseAction
    {
        public abstract bool MeetsCondition();
        public abstract void Invoke(BaseCharacter toAdd);
    }
}