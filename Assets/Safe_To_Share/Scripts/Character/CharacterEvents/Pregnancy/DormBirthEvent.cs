namespace Character.CharacterEvents.Pregnancy
{
    public class DormBirthEvent : SoloEvent
    {
        public override void StartEvent(BaseCharacter actor)
        {
            base.StartEvent(actor);
            
        }

        protected override string LogText(BaseCharacter actor) =>
            $"{actor.Identity.FullName} gave birth a healthy baby.";
    }
}