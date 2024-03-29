﻿using System;
using System.Linq;
using Safe_To_Share.Scripts.Battle.EffectStuff;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.UI {
    public sealed class AssignAbilitySummary : MonoBehaviour {
        [SerializeField] TextMeshProUGUI title, summary, useCost, effectAndDamage;

        void Start() {
            title.text = string.Empty;
            summary.text = string.Empty;
            useCost.text = string.Empty;
            effectAndDamage.text = string.Empty;
            AssignAbilityIcon.AbilityLastHovered += SummaryOf;
        }

        void SummaryOf(Ability obj) {
            title.text = obj.Title;
            summary.text = obj.Desc;
            var useCosts = obj.UseCosts.Select(uc => $"{uc.Type} {uc.Cost}");
            useCost.text = string.Join(Environment.NewLine, useCosts);
        }

        void CalcDamage(Effect effect) { }
    }
}