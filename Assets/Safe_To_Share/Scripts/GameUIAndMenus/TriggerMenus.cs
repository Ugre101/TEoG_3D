using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUIAndMenus
{
    public class TriggerMenus : MonoBehaviour
    {
        [SerializeField] GameCanvas gameCanvas;
        [SerializeField] GameObject looksMenu;
        [SerializeField] GameObject saveMenu;
        [SerializeField] GameObject inventory;
        [SerializeField] GameObject levelMenu;
        [SerializeField] GameObject voreMenu;
        [SerializeField] GameObject bigEventLog;
        [SerializeField] GameObject bigMap;
        [SerializeField] GameObject gameOptions;
        [SerializeField] GameObject questMenu;
        [SerializeField] GameObject essenceMenu;

        void OpenMenu(InputAction.CallbackContext ctx, GameObject menu)
        {
            if (ctx.performed)
                gameCanvas.TriggerMenu(menu);
        }

        public void OpenLooksMenu(InputAction.CallbackContext ctx) => OpenMenu(ctx, looksMenu);

        public void OpenSaveMenu(InputAction.CallbackContext ctx) => OpenMenu(ctx, saveMenu);

        public void OpenInventory(InputAction.CallbackContext ctx) => OpenMenu(ctx, inventory);

        public void OpenLevelMenu(InputAction.CallbackContext ctx) => OpenMenu(ctx, levelMenu);

        public void OpenVoreMenu(InputAction.CallbackContext ctx) => OpenMenu(ctx, voreMenu);

        public void OpenBigEventLog(InputAction.CallbackContext ctx) => OpenMenu(ctx, bigEventLog);

        public void OpenBigMap(InputAction.CallbackContext ctx) => OpenMenu(ctx, bigMap);

        public void OpenGameOptions(InputAction.CallbackContext ctx) => OpenMenu(ctx, gameOptions);

        public void OpenQuestMenu(InputAction.CallbackContext ctx) => OpenMenu(ctx, questMenu);

        public void OpenEssenceMenu(InputAction.CallbackContext ctx) => OpenMenu(ctx, essenceMenu);
    }
}