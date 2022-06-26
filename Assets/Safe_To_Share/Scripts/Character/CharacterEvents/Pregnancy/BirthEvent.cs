namespace Character.CharacterEvents.Pregnancy
{
    public class BirthEvent : SoloEvent
    {
        protected override string LogText(BaseCharacter actor) =>
            $"{actor.Identity.FullName} gave birth a healthy baby.";
    }
}