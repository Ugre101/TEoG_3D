using System;
using Character.PregnancyStuff;

namespace Character.CharacterEvents.Pregnancy
{
    public class PlayerBirthEvent : BirthEvent
    {
        protected override string LogText(BaseCharacter actor) => "You gave birth a healthy baby.";
    }
}