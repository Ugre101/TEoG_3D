using System;
using AvatarStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatsOptionBtn : MonoBehaviour
{
    public event Action<Material> AddMe;
    public event Action<Material> RemoveMe;
    [SerializeField] Button btn;
    [SerializeField] Image btnBackground;
    [SerializeField] TextMeshProUGUI btnTitle;
    bool chosen = true;
    Material material;
    public void Setup(Material mat)
    {
        material = mat;
        btnTitle.text = mat.name;
        UpdateChosenColor();
    }
    void Start()
    {
        btn.onClick.AddListener(Click);
        ChangeAvatarDetails.ToggleAll += ToggledAll;
    }
    void OnDestroy()
    {
        ChangeAvatarDetails.ToggleAll -= ToggledAll;
    }

    private void ToggledAll(bool obj)
    {
        chosen = obj;
        UpdateChosenColor();
    }

    private void Click()
    {
        chosen = !chosen;
        if (chosen)
            AddMe?.Invoke(material);
        else
            RemoveMe?.Invoke(material);
        UpdateChosenColor();
    }

    private void UpdateChosenColor() => btnBackground.color = chosen ? Color.green : Color.gray;
}