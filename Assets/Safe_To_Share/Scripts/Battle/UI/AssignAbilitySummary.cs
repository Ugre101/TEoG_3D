using System;
using System.Collections.Generic;
using System.Linq;
using Battle.EffectStuff;
using Battle.SkillsAndSpells;
using Character.SkillsAndSpells;
using TMPro;
using UnityEngine;

namespace Battle.UI
{
    public class AssignAbilitySummary : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, summary, useCost, effectAndDamage;

        void Start()
        {
            title.text = string.Empty;
            summary.text = string.Empty;
            useCost.text = string.Empty;
            effectAndDamage.text = string.Empty;
            AssignAbilityIcon.AbilityLastHovered += SummaryOf;
        }

        void SummaryOf(Ability obj)
        {
            title.text = obj.Title;
            summary.text = obj.Desc;
            IEnumerable<string> useCosts = obj.UseCosts.Select(uc => $"{uc.Type} {uc.Cost}");
            useCost.text = string.Join(Environment.NewLine, useCosts);
        }

        void CalcDamage(Effect effect)
        {
        }
    }
}