using System.Collections.Generic;
using System.Linq;
using Character.Npc;
using Character.PlayerStuff;
using Character.Service;
using Character.VoreStuff;
using CustomClasses;
using Dialogue;
using Dialogue.UI;
using GameUIAndMenus.DialogueAndEventMenu;
using Items;
using Map;
using Safe_To_Share.Scripts.Static;
using SaveStuff;
using SceneStuff;
using Shop;
using Shop.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus
{
    public class GameCanvas : MonoBehaviour, ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] GameObject gameUI, gameMenus, boatMenu;
        [SerializeField] ShopMenu shopMenu;
        [SerializeField] DialogueMenu dialogueMenu;
        [SerializeField] VoreEventMenu voreEventMenu;
        [SerializeField] ServiceMenu.ServiceMenu serviceMenu;
        [SerializeField] HideGameUI hideGameUI;
        [SerializeField] Button closeMenuBtn;
        IEnumerable<IBlockGameUI> cancelThese;
        bool sceneDirty = true;

        IEnumerable<IBlockGameUI> CancelThese
        {
            get
            {
                if (sceneDirty)
                {
                    cancelThese = FindObjectsOfType<MonoBehaviour>(true).OfType<IBlockGameUI>();
                    sceneDirty = false;
                }

                return cancelThese;
            }
        }

        void Start()
        {
            closeMenuBtn.onClick.AddListener(CloseMenus);
            TriggerBoatMenu.OpenBoatMenu += OpenBoatMenu;
            TradeAbleCharacter.OpenShopMenu += OpenShopMenu;
            LoadManager.LoadedSave += CloseMenus;
            SceneLoader.NewScene += SetDirty;
            BaseDialogue.StartDialogue += OpenDialogueMenu;
            BaseDialogue.StartVoreEvent += OpenVoreEvent;
        }

       

        void OnDestroy()
        {
            TriggerBoatMenu.OpenBoatMenu -= OpenBoatMenu;
            TradeAbleCharacter.OpenShopMenu -= OpenShopMenu;
            LoadManager.LoadedSave -= CloseMenus;
            SceneLoader.NewScene -= SetDirty;
            BaseDialogue.StartDialogue -= OpenDialogueMenu;
            BaseDialogue.StartVoreEvent -= OpenVoreEvent;
        }

        public bool BlockIfActive()
        {
            if (gameMenus.activeInHierarchy || boatMenu.activeInHierarchy)
            {
                CloseMenus();
                return true;
            }

            return false;
        }

        void SetDirty() => sceneDirty = true;

        public void OpenShopMenu(string arg1, List<Item> arg2)
        {
            GameManager.Pause();
            SwitchPanels(shopMenu.gameObject);
            shopMenu.Setup(arg1, arg2);
        }

        public void OpenServiceMenu(string title, OfferServices offers)
        {
            GameManager.Pause();
            SwitchPanels(serviceMenu.gameObject);
            serviceMenu.Setup(title, offers.OfferedServices);
        }

        void OpenBoatMenu() => SwitchPanels(boatMenu);

        public void CloseMenus()
        {
            GameManager.Resume(false);
            SwitchPanels(gameUI);
        }

        public void TriggerMenu(GameObject menu)
        {
            if (CancelThese.Any(blocking => blocking.Block) || menu == null)
                return;
            if (gameMenus.activeSelf && menu.activeSelf)
                CloseMenus();
            else
            {
                GameManager.Pause();
                SwitchPanels(gameMenus);
                gameMenus.transform.SleepChildren(menu);
                closeMenuBtn.gameObject.SetActive(true);
            }
        }

        void SwitchPanels(GameObject panel) => transform.SleepChildren(panel);

        public void HideUI()
        {
            if (gameUI.activeSelf)
                hideGameUI.ToggleHide();
        }

        public void OpenDialogueMenu(BaseDialogue dialogue)
        {
            GameManager.Pause();
            SwitchPanels(dialogueMenu.gameObject);
            dialogueMenu.Setup(dialogue);
        }
        void OpenVoreEvent(BaseDialogue arg1, Player arg2, Prey arg3, VoreOrgan arg4)
        {
            if (voreEventMenu.isActiveAndEnabled) return; // TODO event que system
            GameManager.Pause();
            SwitchPanels(voreEventMenu.gameObject);
            voreEventMenu.Setup(arg1,arg2,arg3,arg4);
        }
    }

    public interface IBlockGameUI
    {
        bool Block { get; }
    }
}