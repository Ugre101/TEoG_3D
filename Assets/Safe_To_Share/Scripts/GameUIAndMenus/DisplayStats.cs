using Character;
using Character.StatsStuff;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus
{
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI statText;

        void PrintStats(BaseCharacter character)
        {
            statText.text = string.Empty;
            foreach ((CharStatType key, CharStat value) in character.Stats.GetCharStats)
                AddToText($"{key}: {value.Value}");

            void AddToText(string toAdd) => statText.text += $"{toAdd}\n";
        }
    }
}