using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SaveStuff
{
    public class ShowSavesAmountSlider : MonoBehaviour
    {
        const string ShowAmountSave = "SaveThisAmountOfSaves";
        public static int ShowAmount = 10;
        [SerializeField] Slider amountToShow;
        [SerializeField] TextMeshProUGUI showAmountText;
        readonly WaitForSecondsRealtime afterSmallDelay = new(0.1f);
        bool firstSlide = true;

        public void SetupSlider(int saves)
        {
            amountToShow.maxValue = saves;
            amountToShow.value = PlayerPrefs.GetInt(ShowAmountSave, ShowAmount);
            showAmountText.text = $"Show {ShowAmount} saves";
            amountToShow.onValueChanged.AddListener(ChangeAmount);
        }

        public void NewMaxAmount(int amount) => amountToShow.maxValue = amount;

        void ChangeAmount(float arg0)
        {
            ShowAmount = Mathf.RoundToInt(arg0);
            PlayerPrefs.SetInt(ShowAmountSave, ShowAmount);
            showAmountText.text = $"Show {ShowAmount} saves";
            if (firstSlide)
                StartCoroutine(AfterSmallDelay());
        }

        public event Action NewShowAmount;

        IEnumerator AfterSmallDelay()
        {
            firstSlide = false;
            yield return afterSmallDelay;
            NewShowAmount?.Invoke();
            firstSlide = true;
        }
    }
}