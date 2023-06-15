using System;
using System.Collections.Generic;
using Character.BodyStuff.BodyBuild;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.BodyStuff {
    [Serializable]
    public class Body : ITickHour {
        // Muscle & fat: 20 != 20kg, 50 "muscle value" is always average muscle independent of height & fat. Likewise with fat.
        [SerializeField] BodyStat muscle, fat, height;
        [SerializeField] Thickset thickset;
        [SerializeField] BaseConstFloatStat fatBurnRate = new(2.5f);
        [SerializeField] BodyMorphs bodyMorphs = new();
        [SerializeField] float skinTone;

        Dictionary<BodyStatType, BodyStat> bodyStats;

        public Body(int muscle, int fat, int height, float thickset = 0f) {
            this.muscle = new BodyStat(muscle);
            this.fat = new BodyStat(fat);
            this.height = new BodyStat(height);
            this.thickset = new Thickset(thickset);
        }

        public Body() : this(20, 20, 160) { }

        public float FatWeight => Height.Value * (Fat.Value / 200f);

        public float MuscleWeight => Height.Value * (Muscle.Value / 200f);

        // Need improvment
        public float Weight => FatWeight + MuscleWeight + Height.Value * 0.20f;

        public BodyStat Muscle => muscle;

        public BodyStat Fat => fat;

        public BodyStat Height => height;

        public Dictionary<BodyStatType, BodyStat> BodyStats =>
            bodyStats ??= new Dictionary<BodyStatType, BodyStat> {
                { BodyStatType.Fat, Fat },
                { BodyStatType.Height, Height },
                { BodyStatType.Muscle, Muscle },
            };

        public BaseConstFloatStat FatBurnRate => fatBurnRate;

        public BodyMorphs Morphs => bodyMorphs;

        public Thickset Thickset => thickset;

        public float SkinTone {
            get => skinTone;
            set => skinTone = Mathf.Clamp(value, 0f, 1f);
        }


        public bool TickHour(int ticks = 1) {
            var change = false;
            foreach (var bodyStatsValue in BodyStats.Values)
                if (bodyStatsValue.TickHour(ticks))
                    change = true;
            FatBurnRate.TickHour(ticks);
            if (Thickset.TickHour(ticks))
                change = true;
            return change;
        }

        public void Loaded(params AssignBodyMod[] mods) {
            foreach (var mod in mods)
                BodyStats[mod.Type].Mods.AddStatMod(mod.Mod);
            Morphs.Loaded();
        }
    }
}