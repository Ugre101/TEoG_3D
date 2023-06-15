using System;
using Character;
using Character.VoreStuff;

namespace Dialogue.DialogueActions.Vore {
    [Serializable]
    public class ReleasePrey : DialogueVoreAction {
        public override bool MeetsCondition() => true;

        public override void Invoke(BaseCharacter toAdd, Prey prey, VoreOrgan container) {
            container.ReleasePrey(prey.Identity.ID);
        }
    }
}