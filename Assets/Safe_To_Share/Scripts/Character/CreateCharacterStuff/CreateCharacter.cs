﻿using Character.LevelStuff;
using Character.Race.Races;
using Character.StatsStuff;
using Items;
using Safe_To_Share.Scripts.CustomClasses;

namespace Character.CreateCharacterStuff {
    public readonly struct CreateCharacter {
        public CreateCharacter(StartIdentity identity, Stats stats, string[] startAbilities,
                               AddAmountOf<Item>[] startItems,
                               BasicRace startRace, StartGender startGender, StartBody startBody,
                               BasicPerk[] startPerks, StartHair hair,
                               StartSkinColor skin) {
            Identity = identity;
            Stats = stats;
            StartAbilities = startAbilities;
            StartItems = startItems;
            StartRace = startRace;
            StartGender = startGender;
            StartBody = startBody;
            StartPerks = startPerks;
            Hair = hair;
            Skin = skin;
        }

        public StartIdentity Identity { get; }

        public Stats Stats { get; }

        public string[] StartAbilities { get; }

        public AddAmountOf<Item>[] StartItems { get; }
        public BasicPerk[] StartPerks { get; }
        public BasicRace StartRace { get; }
        public StartGender StartGender { get; }
        public StartBody StartBody { get; }
        public StartHair Hair { get; }
        public StartSkinColor Skin { get; }
    }
}