using Character;
using Character.PlayerStuff;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public class AfterBattleMainUI : MonoBehaviour
    {
        [SerializeField] AfterBattleActorInfo playerInfo;
        [SerializeField] AfterBattleActorInfo enemyInfo;
        [SerializeField] SexActionButtons buttons;
        [SerializeField] AfterBattleLog log;
        readonly WaitForSeconds waitForSeconds = new(0.8f);

        bool leaving;
        Player player;
        bool preloading;
        public static AfterBattleMainUI Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Debug.LogError("Duplicate after battle UI");
        }

        public void Setup(Player parPlayer, BaseCharacter buttonOwner, BaseCharacter partner)
        {
            player = parPlayer;
            buttons.FirstSetup(buttonOwner, partner);
        }

        public void NoPartnerRefresh(BaseCharacter buttonOwner) => buttons.Refresh(buttonOwner);

        public void RefreshButtons(BaseCharacter buttonOwner, BaseCharacter partner) =>
            buttons.Refresh(buttonOwner, partner);

        public void SetupPlayer(BaseCharacter character) => playerInfo.Setup(character);

        public void SetupPartner(BaseCharacter character) => enemyInfo.Setup(character);

        public void LogText(SexActData data)
        {
            if (data.TitleText.Length > 0)
                log.AddNewText(data.TitleText);
            if (data.AfterText.Count > 0)
                foreach (string s in data.AfterText)
                    log.AddNewText(s);
        }

        public void Leave()
        {
            if (leaving)
                return;
            leaving = true;
            SceneLoader.Instance.LeaveAfterBattle(player);
        }
    }
}