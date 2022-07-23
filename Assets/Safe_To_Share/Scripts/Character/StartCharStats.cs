using Character.StatsStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCharStats : MonoBehaviour
{
    [SerializeField] int startAmount = 20;
    [SerializeField] Stats stats = new(5, 5, 5, 5, 5);
    [SerializeField] StartCharStatSlider charStatSlider;
    [SerializeField] Transform content;

    void Start()
    {
        foreach ((CharStatType key, CharStat value) in stats.GetCharStats)
            Instantiate(charStatSlider, content).Setup(value, key);
    }
}

public class StartCharStatSlider : MonoBehaviour
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