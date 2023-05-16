using Character.StatsStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class StartCharStatSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI title;

    CharStat charStat;
    CharStatType charStatType;

    public void Setup(CharStat stat, CharStatType statType)
    {
        charStat = stat;
        charStatType = statType;
        UpdateText();
        slider.value = stat.BaseValue;
        slider.onValueChanged.AddListener(Change);
    }

    void Change(float arg0)
    {
        charStat.BaseValue = (int)arg0;
        UpdateText();
    }

    void UpdateText() => title.text = $"{charStatType} {charStat.BaseValue}";
}