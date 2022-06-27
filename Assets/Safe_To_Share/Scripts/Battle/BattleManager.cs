using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battle.CombatantStuff;
using Battle.SkillsAndSpells;
using Battle.UI;
using Character;
using Character.EnemyStuff;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Character.SkillsAndSpells;
using Currency;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static CombatCharacter CurrentPlayerControlled;
        [SerializeField] CombatantTeam playerTeam, enemyTeam;

        [SerializeField] BattleTarget battleTarget;
        BaseCharacter[] enemyTeamChars;
        Player player;
        ControlledCharacter[] playerTeamChars;

        [SerializeField] BattleAI battleAI;
        bool waitingForPlayerInput;
        List<CombatCharacter> whoseTurn;
        public static BattleManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            AttackBtn.PlayerAction += HandlePlayerAction;
            AttackBtn.BoundAbility += BindToActivePlayer;
            SceneLoader.ActionSceneLoaded += Setup;
        }

        void OnDestroy()
        {
            AttackBtn.PlayerAction -= HandlePlayerAction;
            AttackBtn.BoundAbility -= BindToActivePlayer;
            SceneLoader.ActionSceneLoaded -= Setup;
        }

        void HandlePlayerAction(Ability ability)
        {
            if (!waitingForPlayerInput)
                return;
            StartCoroutine(PlayerAction(ability));
            waitingForPlayerInput = false;
        }

        static void BindToActivePlayer(int arg1, Ability arg2)
        {
            if (CurrentPlayerControlled.Character is ControlledCharacter controlledCharacter)
                controlledCharacter.AndSpellBook.BoundAbilities[arg1] = arg2.Guid;
        }


        public void Setup(Player parPlayer, BaseCharacter[] enemies, BaseCharacter[] allies,bool boss)
        {
            player = parPlayer;
            playerTeamChars = new ControlledCharacter[] { player, };
            if (allies != null)
            {
                // TODO
            }

            enemyTeamChars = enemies;
            whoseTurn = new List<CombatCharacter>();


            SetupTeam(playerTeam, playerTeamChars, true);
            BuildSpeed(); // Favour player team
            SetupTeam(enemyTeam, enemyTeamChars, false);

            void SetupTeam(CombatantTeam team, IEnumerable<BaseCharacter> charArray, bool ally)
            {
                team.FirstSetup();
                foreach (BaseCharacter character in charArray)
                    whoseTurn.Add(new CombatCharacter(team.SetupTeam(character), character, ally));
            }

            BattleUIManager.Instance.Setup(whoseTurn);
            NextTurn();
            transform.AwakeChildren();
            StartCoroutine(BattleSceneManager.PreLoadAfterBattle());
        }


        IEnumerator PlayerAction(Ability obj)
        {
            yield return obj.UseEffect(CurrentPlayerControlled, battleTarget.EnemyTargeted);
            battleTarget.EnemyTargeted.Combatant.StopTargeting();
            NextTurn();
        }

        void NextTurn()
        {
            if (HaveATeamWon())
                return;
            BuildSpeed();
            CombatCharacter next = whoseTurn.Where(cc => !cc.Character.Stats.Dead)
                .OrderByDescending(t => t.SpeedAccumulated).FirstOrDefault();
            if (next == null)
            {
                BattleSceneManager.Leave(player);
                // TODO Show error message
                return;
            }

            BattleUIManager.Instance.NewTurn();
            if (next.Ally)
                HandleTurnAlly(next.MyTurn());
            else
                StartCoroutine(HandleTurnEnemy(next.MyTurn()));
        }


        bool HaveATeamWon()
        {
            bool alliesLeft = whoseTurn.Where(a => a.Ally).Any(a => !a.Character.Stats.Dead);
            bool enemiesLeft = whoseTurn.Where(e => !e.Ally).Any(e => !e.Character.Stats.Dead);
            if (alliesLeft && enemiesLeft)
                return false;
            if (enemiesLeft)
                BattleSceneManager.Defeat();
            else if (alliesLeft)
                Victory();
            else
                Leave();
            return true;
        }

        void Victory()
        {
            BattleSceneManager.Victory();
            foreach (BaseCharacter teamChar in enemyTeamChars)
                if (teamChar is Enemy enemy)
                    HandleDefeatedEnemy(player, enemy);
        }

        static void HandleDefeatedEnemy(Player player, Enemy enemy)
        {
            player.LevelSystem.GainExp(enemy.Reward.ExpReward);
            PlayerGold.GoldBag.GainGold(enemy.Reward.GoldReward);
            enemy.Stats.FullRecovery(80);
        }

        public void Leave()
        {
            battleAI.SetLoadedFalse();
            BattleSceneManager.Leave(player);
        }


        void BuildSpeed()
        {
            foreach (CombatCharacter nextTurn in whoseTurn)
                nextTurn.NewTurn();
        }

        void HandleTurnAlly(CombatCharacter myTurn)
        {
            CurrentPlayerControlled = myTurn;
            // Linq to get array of most threatening enemies

            var possibleEnemyTargets = whoseTurn.Where(t => !t.Ally && !t.Character.Stats.Dead)
                .OrderByDescending(t => t.Threat).GroupBy(t => t.Threat).SelectMany(g => g).ToArray();

            battleTarget.SetPossibleTargets(possibleEnemyTargets);

            if (battleTarget.EnemyTargeted == null)
                HaveATeamWon();
            else
                battleTarget.EnemyTargeted.Combatant.Target();

            BattleUIManager.Instance.HandleTurnAlly(myTurn);
            waitingForPlayerInput = true;
        }


        IEnumerator HandleTurnEnemy(CombatCharacter myTurn)
        {
            BattleUIManager.Instance.EnemyTurn();
            // Attack player tea
            CombatCharacter target = whoseTurn.Where(c => c.Ally && !c.Character.Stats.Dead).OrderBy(t => t.Threat)
                .FirstOrDefault();
            if (target == null)
                HaveATeamWon();
            else
            {
                target.Combatant.Target();
                yield return battleAI.HandleTurn(myTurn, target);
                target.Combatant.StopTargeting();
                NextTurn();
            }
        }

        public void GoToDefeat() => BattleSceneManager.GoToDefeat(player, enemyTeamChars);

        public void GoToAfterBattle() => BattleSceneManager.GoToAfterBattle(player, enemyTeamChars);

        public async Task Hide()
        {
            SceneLoader.ActionSceneLoaded -= Setup;
            gameObject.SetActive(false);
            await Task.Yield();
        }
    }
}