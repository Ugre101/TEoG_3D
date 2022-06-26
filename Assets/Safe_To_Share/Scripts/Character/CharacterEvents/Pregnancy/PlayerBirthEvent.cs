namespace Character.CharacterEvents.Pregnancy
{
    public class PlayerBirthEvent : SoloEvent
    {
        protected override string LogText(BaseCharacter actor) => "You gave birth a healthy baby.";
    }
}