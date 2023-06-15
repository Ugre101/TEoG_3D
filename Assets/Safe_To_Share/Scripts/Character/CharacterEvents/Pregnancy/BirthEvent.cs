using System;
using System.Collections.Generic;
using Character.PregnancyStuff;

namespace Character.CharacterEvents.Pregnancy {
    public class BirthEvent : SoloEvent {
        public event Action<BaseCharacter, IEnumerable<Fetus>> TriggerBirthMenu;

        public void StartEvent(BaseCharacter character, IEnumerable<Fetus> fetus) {
            TriggerBirthMenu?.Invoke(character, fetus);
        }

        protected override string LogText(BaseCharacter actor) =>
            $"{actor.Identity.FullName} gave birth a healthy baby.";
    }
}