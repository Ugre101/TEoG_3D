using System;
using Character;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.AfterBattle.UI;
using SceneStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.UI
{
    public class DefeatMainUI : MonoBehaviour
    {
        [SerializeField] AfterBattleActorInfo playerInfo;
        [SerializeField] AfterBattleActorInfo enemyInfo;
        [SerializeField] Button resist, giveIn;
        [SerializeField] Button continueBtn, leaveBtn;
        [SerializeField] TextMeshProUGUI textLog;
        readonly WaitForSeconds waitForSeconds = new(0.8f);

        bool alreadyLeaving;
        Player player;
        bool preloading;
        public static DefeatMainUI Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError("Duplicate defeat main UI");
                Destroy(gameObject);
            }
        }

        public static event Action Resist, GiveIn, Continue;

        public void Setup(Player parPlayer)
        {
            player = parPlayer;
            // buttons.FirstSetup(buttonOwner, partner);
            resist.onClick.AddListener(InvokeResist);
            giveIn.onClick.AddListener(InvokeGiveIn);
            continueBtn.onClick.AddListener(InvokeContiune);
            leaveBtn.onClick.AddListener(Leave);
        }

        static void InvokeContiune() => Continue?.Invoke();
        static void InvokeGiveIn() => GiveIn?.Invoke();
        static void InvokeResist() => Resist?.Invoke();


        public void SetupPlayer(BaseCharacter character) => playerInfo.Setup(character);

        public void SetupPartner(BaseCharacter character) => enemyInfo.Setup(character);

        public void Leave()
        {
            if (alreadyLeaving)
                return;
            alreadyLeaving = true;
            SceneLoader.Instance.LoadLastLocation(player);
        }


        public void SetupNode(string node)
        {
            giveIn.gameObject.SetActive(true);
            resist.gameObject.SetActive(true);
            leaveBtn.gameObject.SetActive(false);
            continueBtn.gameObject.SetActive(false);
            textLog.text = node;
        }

        public void PrintNodeEffect(string node)
        {
            giveIn.gameObject.SetActive(false);
            resist.gameObject.SetActive(false);
            leaveBtn.gameObject.SetActive(false);
            continueBtn.gameObject.SetActive(true);
            textLog.text = node;
        }

        public void ShowLeaveBtn(bool only)
        {
            leaveBtn.gameObject.SetActive(true);
            if (only)
            {
                giveIn.gameObject.SetActive(false);
                resist.gameObject.SetActive(false);
                continueBtn.gameObject.SetActive(false);
            }
        }

        public void Resisted()
        {
        }

        public void FailedResist()
        {
        }

        public void StartNode(string startNode)
        {
            giveIn.gameObject.SetActive(false);
            resist.gameObject.SetActive(false);
            leaveBtn.gameObject.SetActive(false);
            continueBtn.gameObject.SetActive(true);
            textLog.text = startNode;
        }
    }
}