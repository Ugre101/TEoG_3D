using Character.GenderStuff;

namespace Character.CharacterEvents.Vore {
    public class PreyDigestedStomach : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"{actor.Identity.FirstName} has digested {partner.Identity.FirstName}";
    }

    public sealed class PlayerDigestedPreyStomach : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"you have fully digested {partner.Identity.FirstName}";
    }

    public sealed class PlayerAbsorbedPreyStomach : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"You have fully absorbed {partner.Identity.FirstName}, {partner.Gender.HeShe()} are now a part of you.";
    }

    public sealed class PlayerBoobsDigestion : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"{partner.Identity.FirstName} has been melted into {actor.SexualOrgans.Boobs.FluidType} inside your breasts.";
    }

    public sealed class PlayerBoobsTransformed : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"{partner.Identity.FirstName} has fully merged with your breasts.";
    }

    public sealed class PlayerBallsDigestion : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"Your balls have churned {partner.Identity.FirstName} into {actor.SexualOrgans.Balls.FluidType}";
    }

    public sealed class PlayerUnbirthDigestion : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"Your vagina drips of the {actor.SexualOrgans.Vaginas.FluidType} {partner.Identity.FirstName} have melted into.";
    }

    public sealed class PlayerRebirthedPrey : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"Your vagina drips of the {actor.SexualOrgans.Vaginas.FluidType} {partner.Identity.FirstName} have melted into.";
    }

    public sealed class PlayerCockTransformedPrey : DuoEvent {
        protected override string LogText(BaseCharacter actor, BaseCharacter partner) =>
            $"{partner.Identity.FirstName}'s consciousness have fully merged with your cock as they are now a part of it.";
    }
}