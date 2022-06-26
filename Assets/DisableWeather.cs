using Gaia;
using UnityEngine;
using UnityEngine.UI;

public class DisableWeather : MonoBehaviour
{
    const string SaveName = "GaiaWeatherActive";
    [SerializeField] Toggle toggle;
    void Start()
    {
        toggle.isOn = PlayerPrefs.GetInt(SaveName, 1) == 1;
        toggle.onValueChanged.AddListener(ToggleWeather);
    }

    static void ToggleWeather(bool arg0)
    {
        PlayerPrefs.SetInt(SaveName,arg0 ? 1 : 0);
        ProceduralWorldsGlobalWeather.Instance.SetWeatherStatus(arg0);
    }

    public static void LoadWeatherSettings() => ProceduralWorldsGlobalWeather.Instance.SetWeatherStatus(PlayerPrefs.GetInt(SaveName,1) == 1);
}