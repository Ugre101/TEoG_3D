using System;
using System.Collections.Generic;
using Character.CharacterEvents.Pregnancy;
using Character.CharacterEvents.Vore;
using Character.Organs;
using Character.PregnancyStuff;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina;
using Safe_To_Share.Scripts.Static;

namespace Character.CharacterEvents
{
    public static class CharacterEvents
    {
        public static BirthEvent BirthEvent { get; } = new();

        public static class PlayerEvents
        {
            public static PlayerBirthEvent PlayerBirthEvent { get; } = new();

            public static class VoreEvents
            {
                static Dictionary<SexualOrganType, Dictionary<string, DuoEvent>> organDigestionDict;
                static Dictionary<string, DuoEvent> voreStomachEvents;
                static Dictionary<SexualOrganType, Dictionary<string, DuoEvent>> organDigestionProgressDict;

                public static Dictionary<SexualOrganType, Dictionary<string, DuoEvent>> VoreOrganEvents =>
                    organDigestionDict ??= new Dictionary<SexualOrganType, Dictionary<string, DuoEvent>>
                    {
                        {
                            SexualOrganType.Balls, new Dictionary<string, DuoEvent>
                            {
                                { VoreOrganDigestionMode.Digestion, new PlayerBallsDigestion() },
                            }
                        },
                        {
                            SexualOrganType.Boobs, new Dictionary<string, DuoEvent>
                            {
                                { VoreOrganDigestionMode.Digestion, new PlayerBoobsDigestion() },
                                { VoreOrganDigestionMode.Absorption, new PlayerBoobsTransformed() },
                            }
                        },
                        {
                            SexualOrganType.Dick, new Dictionary<string, DuoEvent>
                            {
                                { VoreOrganDigestionMode.Absorption, new PlayerCockTransformedPrey() },
                            }
                        },
                        {
                            SexualOrganType.Vagina, new Dictionary<string, DuoEvent>
                            {
                                { VoreOrganDigestionMode.Digestion, new PlayerUnbirthDigestion() },
                                { VaginaDigestionModes.Rebirth, new PlayerRebirthedPrey() },
                            }
                        },
                    };

                public static Dictionary<string, DuoEvent> VoreStomachEvents => voreStomachEvents ??=
                    new Dictionary<string, DuoEvent>
                    {
                        { VoreOrganDigestionMode.Digestion, new PlayerDigestedPreyStomach() },
                        { VoreOrganDigestionMode.Absorption, AbsorbedPreyStomach },
                    };

                static PlayerAbsorbedPreyStomach AbsorbedPreyStomach { get; } = new();

                public static Dictionary<SexualOrganType, Dictionary<string, DuoEvent>> VoreOrganProgressEvents =>
                    organDigestionProgressDict ??= new Dictionary<SexualOrganType, Dictionary<string, DuoEvent>>();
            }
        }

        public static class EnemyVore
        {
        }
    }

    public abstract class SoloEvent
    {
        // Action trigger a event menu
        protected abstract string LogText(BaseCharacter actor);

        public virtual void StartEvent(BaseCharacter actor) => EventLog.AddEvent(LogText(actor));
    }

    public abstract class DuoEvent
    {
        // Action trigger a event menu
        protected abstract string LogText(BaseCharacter actor, BaseCharacter partner);

        public void StartEvent(BaseCharacter actor, BaseCharacter partner) =>
            EventLog.AddEvent(LogText(actor, partner));
    }
}