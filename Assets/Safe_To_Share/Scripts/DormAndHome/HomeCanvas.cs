using AvatarStuff.Holders;
using Character;
using Character.PlayerStuff;
using CustomClasses;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DormAndHome
{
    public class HomeCanvas : MonoBehaviour, ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] GameObject panel;
        [SerializeField] Button sleepBtn;
        [SerializeField] TextMeshProUGUI sleepCooldown;
        Player player;

        void Start() => sleepBtn.onClick.AddListener(SleepAtHome);

        public bool BlockIfActive()
        {
            if (panel.activeSelf)
            {
                Leave();
                return true;
            }

            return false;
        }

        void SleepAtHome()
        {
            // TODO Upgradable bedroom
            player.Sleep(100);
            CanSleep();
        }

        public void Enter(Player component)
        {
            player = component;
            GameManager.Pause();
            panel.SetActive(true);
            CanSleep();
        }

        void CanSleep()
        {
            if (!player.PlayerCanSleep())
            {
                sleepBtn.interactable = false;
                sleepCooldown.text =
                    $"{SleepExtensions.HoursBeforeCanSleep(player)}h before you can sleep again.";
                return;
            }

            sleepBtn.interactable = true;
            sleepCooldown.text = string.Empty;
        }

        public void Leave()
        {
            GameManager.Resume(false);
            panel.SetActive(false);
        }
    }
}