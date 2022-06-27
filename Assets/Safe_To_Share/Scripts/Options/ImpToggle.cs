using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.UI;

public class ImpToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out Toggle toggle))
        {
            toggle.isOn = !MetricOrImperial.Metric.Enabled;
            toggle.onValueChanged.AddListener((arg0 => MetricOrImperial.Metric.Enabled = !arg0));
        } else gameObject.SetActive(false);
    }
}
