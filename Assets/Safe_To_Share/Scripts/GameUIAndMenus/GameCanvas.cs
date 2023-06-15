using System.Collections.Generic;
using System.Linq;
using Character;
using Character.CharacterEvents;
using Character.Npc;
using Character.PlayerStuff;
using Character.PregnancyStuff;
using Character.Service;
using Character.VoreStuff;
using CustomClasses;
using Dialogue;
using Items;
using Map;
using Safe_To_Share.Scripts.GameUIAndMenus.DialogueAndEventMenu;
using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory;
using Safe_To_Share.Scripts.Static;
using SaveStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class GameCanvas : MonoBehaviour, ICancelMeBeforeOpenPauseMenu {
        [SerializeField] GameObject gameUI, gameMenus, boatMenu;
        [SerializeField] ShopMenu.ShopMenu shopMenu;
        [SerializeField] DialogueMenu dialogueMenu;
        [SerializeField] VoreEventMenu voreEventMenu;
        [SerializeField] ServiceMenu.ServiceMenu serviceMenu;
        [SerializeField] HideGameUI hideGameUI;
        [SerializeField] Button closeMenuBtn;
        [SerializeField] InventoryMenu2 inventoryMenu;
        [SerializeField] BirthEventMenu birthEventMenu;
        [SerializeField] SleepCanvas sleepCanvas;
        IEnumerable<BlockGameUI> cancelThese;

        void Start() {
            closeMenuBtn.onClick.AddListener(CloseMenus);
            TriggerBoatMenu.OpenBoatMenu += OpenBoatMenu;
            TradeAbleCharacter.OpenShopMenu += OpenShopMenu;
            LoadManager.LoadedSave += CloseMenus;
            BaseDialogue.StartDialogue += OpenDialogueMenu;
            BaseDialogue.StartVoreEvent += OpenVoreEvent;
            GameUIManager.HideGameUI += ForceHide;
            GameUIManager.SleepEffect += GameUIManagerOnSleepEffect;
            InventoryChest.OpenInventory += OpenSecInv;
            CharacterEvents.PlayerEvents.PlayerBirthEvent.TriggerBirthMenu += PlayerBirthEventOnTriggerBirthMenu;
            SwitchPanels(gameUI);
        }


        void OnDestroy() {
            TriggerBoatMenu.OpenBoatMenu -= OpenBoatMenu;
            TradeAbleCharacter.OpenShopMenu -= OpenShopMenu;
            LoadManager.LoadedSave -= CloseMenus;
            BaseDialogue.StartDialogue -= OpenDialogueMenu;
            BaseDialogue.StartVoreEvent -= OpenVoreEvent;
            GameUIManager.HideGameUI -= ForceHide;
            GameUIManager.SleepEffect -= GameUIManagerOnSleepEffect;
            InventoryChest.OpenInventory -= OpenSecInv;
            CharacterEvents.PlayerEvents.PlayerBirthEvent.TriggerBirthMenu -= PlayerBirthEventOnTriggerBirthMenu;
        }

        public bool BlockIfActive() {
            if (!gameMenus.activeInHierarchy && !boatMenu.activeInHierarchy) return false;
            CloseMenus();
            return true;
        }

        void GameUIManagerOnSleepEffect() {
            SwitchPanels(sleepCanvas.gameObject);
            sleepCanvas.Sleep();
        }

        void PlayerBirthEventOnTriggerBirthMenu(BaseCharacter arg1, IEnumerable<Fetus> arg2) {
            SwitchPanels(birthEventMenu.gameObject);
            birthEventMenu.PlayerMotherBirthEvent(arg1, arg2);
        }

        void OpenSecInv(Inventory obj) {
            TriggerMenu(inventoryMenu.gameObject);
            inventoryMenu.SetupSecondaryInventory(obj);
        }

        void ForceHide(bool obj) {
            if (obj)
                hideGameUI.ForceHide();
            else
                hideGameUI.StopForceHide();
        }


        public void OpenShopMenu(string arg1, List<Item> arg2) {
            GameManager.Pause();
            SwitchPanels(shopMenu.gameObject);
            shopMenu.Setup(arg1, arg2);
        }

        public void OpenServiceMenu(string title, OfferServices offers) {
            GameManager.Pause();
            SwitchPanels(serviceMenu.gameObject);
            serviceMenu.Setup(title, offers.OfferedServices);
        }

        void OpenBoatMenu() {
            GameManager.Pause();
            SwitchPanels(boatMenu);
        }

        public void CloseMenus() {
            GameManager.Resume(false);
            SwitchPanels(gameUI);
        }

        public void TriggerMenu(GameObject menu) {
            if (GameUIManager.BlockList.Any() || menu == null)
                return;
            if (gameMenus.activeSelf && menu.activeSelf) {
                CloseMenus();
            } else {
                GameManager.Pause();
                SwitchPanels(gameMenus);
                gameMenus.transform.SleepChildren(menu);
                closeMenuBtn.gameObject.SetActive(true);
            }
        }

        void SwitchPanels(GameObject panel) => transform.SleepChildren(panel);

        public void HideUI() {
            if (gameUI.activeSelf)
                hideGameUI.ToggleHide();
        }

        public void OpenDialogueMenu(BaseDialogue dialogue) {
            GameManager.Pause();
            SwitchPanels(dialogueMenu.gameObject);
            dialogueMenu.Setup(dialogue);
        }

        void OpenVoreEvent(BaseDialogue arg1, Player arg2, Prey arg3, VoreOrgan arg4) {
            if (voreEventMenu.isActiveAndEnabled) return; // TODO event que system
            GameManager.Pause();
            SwitchPanels(voreEventMenu.gameObject);
            voreEventMenu.Setup(arg1, arg2, arg3, arg4);
        }
    }
}