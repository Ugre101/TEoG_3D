using System.Collections.Generic;
using System.Threading.Tasks;
using Battle.SkillsAndSpells;
using Character;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Battle.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] AttackButtons attackButtons;
        [SerializeField] GameObject winPanel, defeatPanel;
        [SerializeField] WhoseTurnIcons turnIcons;
        [SerializeField] GameObject sfwWinPanel;
        [SerializeField] bool sfw;
        [SerializeField] AssignAbilityMenu abilityMenu;
        public static BattleUIManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start() => AttackBtn.PlayerAction += HandlePlayerAction;

        void OnDestroy()
        {
            AttackBtn.PlayerAction -= HandlePlayerAction;
            abilityMenu.OnDestroy();
        }

        public void Setup(List<CombatCharacter> combatCharacters)
        {
            attackButtons.FirstSetup();
            abilityMenu.FirstSetup();
            turnIcons.FirstSetup();
            transform.AwakeChildren();
            foreach (CombatCharacter combatCharacter in combatCharacters)
                turnIcons.AddCombatant(combatCharacter);
        }


        public void Victory()
        {
            if (sfw)
                sfwWinPanel.SetActive(true);
            else
                winPanel.SetActive(true);
        }

        public void Defeat() => defeatPanel.SetActive(true);

        public void NewTurn() => turnIcons.RefreshList();

        void HandlePlayerAction(Ability ability) => attackButtons.gameObject.SetActive(false);

        public void EnemyTurn() => attackButtons.gameObject.SetActive(false);

        public void HandleTurnAlly(CombatCharacter myTurn)
        {
            attackButtons.gameObject.SetActive(true);
            attackButtons.Setup(myTurn.Character as ControlledCharacter);
        }

        public async Task Hide()
        {
            gameObject.SetActive(false);
            await Task.Yield();
        }
    }
}