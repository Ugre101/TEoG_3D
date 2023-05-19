using System.Text;
using Character;
using Character.StatsStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public sealed class DisplayStats : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI statText;

        void PrintStats(BaseCharacter character)
        {
            StringBuilder sb = new StringBuilder();
            foreach ((CharStatType key, CharStat value) in character.Stats.GetCharStats)
                sb.AppendLine($"{key}: {value.Value}");
            statText.text = sb.ToString();
        }
    }
}