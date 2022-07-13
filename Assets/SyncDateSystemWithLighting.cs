using System.Collections;
using Gaia;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

public class SyncDateSystemWithLighting : MonoBehaviour
{
    static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
    static GaiaTimeOfDay TimeOfDay => GaiaGlobal.Instance.GaiaTimeOfDayValue;

    void OnEnable()
    {
        StartCoroutine(SyncTime());
        DateSystem.NewMinute += UpdateMinutes;
        DateSystem.NewHour += UpdateHour;
    }

    static IEnumerator SyncTime()
    {
        yield return WaitForEndOfFrame; // Let things start
        TimeOfDay.m_todHour = DateSystem.Hour;
        TimeOfDay.m_todMinutes = DateSystem.Minute;
    }

    void OnDisable() => UnSub();

    void OnDestroy() => UnSub();

    private static void UnSub()
    {
        DateSystem.NewMinute -= UpdateMinutes;
        DateSystem.NewHour -= UpdateHour;
    }

    static void UpdateHour(int obj)
    {
        TimeOfDay.m_todHour = obj;
        GaiaGlobal.Instance.UpdateGaiaTimeOfDay(false);
    }

    static int lastMin;
    static void UpdateMinutes(int obj)
    {
        int abs = Mathf.Abs(obj - lastMin);
        if (abs <= 2) return;
        lastMin = obj;
        TimeOfDay.m_todMinutes = obj;
        GaiaGlobal.Instance.UpdateGaiaTimeOfDay(false);
    }
}