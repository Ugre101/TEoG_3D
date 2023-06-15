using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Character;
using Character.EnemyStuff;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Safe_To_Share.Scripts.Battle.CombatantStuff;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using Safe_To_Share.Scripts.Battle.UI;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;

namespace Battle {
    public sealed class BattleManager : MonoBehaviour {
        public static CombatCharacter CurrentPlayerControlled;
        [SerializeField] CombatantTeam playerTeam, enemyTeam;

        [SerializeField] BattleTarget battleTarget;

        [SerializeField] BattleAI battleAI;
        BaseCharacter[] enemyTeamChars;
        Player player;
        ControlledCharacter[] playerTeamChars;
        bool waitingForPlayerInput;
        List<CombatCharacter> whoseTurn;
        public static BattleManager Instance { get; private set; }

        void Awake() {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start() {
            AttackBtn.PlayerAction += HandlePlayerAction;
            AttackBtn.BoundAbility += BindToActivePlayer;
            SceneLoader.ActionSceneLoaded += Setup;
        }

        void OnDestroy() {
            AttackBtn.PlayerAction -= HandlePlayerAction;
            AttackBtn.BoundAbility -= BindToActivePlayer;
            SceneLoader.ActionSceneLoaded -= Setup;
            battleAI.CleanUp();
        }

        void HandlePlayerAction(Ability ability) {
            if (!waitingForPlayerInput)
                return;
            StartCoroutine(PlayerAction(ability));
            waitingForPlayerInput = false;
        }

        static void BindToActivePlayer(int arg1, Ability arg2) {
            if (CurrentPlayerControlled.Character is ControlledCharacter controlledCharacter)
                controlledCharacter.AndSpellBook.BoundAbilities[arg1] = arg2.Guid;
        }


        public async void Setup(Player parPlayer, BaseCharacter[] enemies, BaseCharacter[] allies, bool boss) {
            player = parPlayer;
            playerTeamChars = new ControlledCharacter[] { player, };
            if (allies != null) {
                // TODO
            }

            enemyTeamChars = enemies;
            whoseTurn = new List<CombatCharacter>();

            transform.AwakeChildren();
            await SetupTeam(playerTeam, playerTeamChars, true);
            BuildSpeed(); // Favour player team
            await SetupTeam(enemyTeam, enemyTeamChars, false);

            async Task SetupTeam(CombatantTeam team, IEnumerable<BaseCharacter> charArray, bool ally) {
                team.FirstSetup();
                foreach (var character in charArray) {
                    var setupTeam = await team.SetupTeam(character);
                    whoseTurn.Add(new CombatCharacter(setupTeam, character, ally));
                }
            }

            BattleUIManager.Instance.Setup(whoseTurn);
            NextTurn();
            StartCoroutine(BattleSceneManager.PreLoadAfterBattle());
        }


        IEnumerator PlayerAction(Ability obj) {
            yield return obj.UseEffect(CurrentPlayerControlled, battleTarget.EnemyTargeted);
            battleTarget.EnemyTargeted.Combatant.StopTargeting();
            NextTurn();
        }

        void NextTurn() {
            var someOneDefeated = whoseTurn.RemoveAll(c => c.Character.Stats.Dead) > 0;
            if (someOneDefeated && HaveATeamWon())
                return;
            BuildSpeed();

            whoseTurn.Sort((cc1, cc2) => cc2.SpeedAccumulated.CompareTo(cc1.SpeedAccumulated));
            var next = whoseTurn[0];
            if (next == null) {
                BattleSceneManager.Leave(player);
                // TODO Show error message or Draw
                return;
            }

            BattleUIManager.Instance.NewTurn();
            if (next.Ally)
                HandleTurnAlly(next.MyTurn());
            else
                StartCoroutine(HandleTurnEnemy(next.MyTurn()));
        }


        bool HaveATeamWon() {
            var alliesLeft = whoseTurn.Any(a => a.Ally);
            var enemiesLeft = whoseTurn.Any(e => !e.Ally);
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

        void Victory() {
            BattleSceneManager.Victory();
            foreach (var teamChar in enemyTeamChars)
                if (teamChar is Enemy enemy)
                    HandleDefeatedEnemy(player, enemy);
        }

        static void HandleDefeatedEnemy(Player player, Enemy enemy) {
            player.LevelSystem.GainExp(enemy.Reward.ExpReward);
            PlayerGold.GoldBag.GainGold(enemy.Reward.GoldReward);
            enemy.Stats.FullRecovery(80);
            enemy.SetDefeated(true);
        }

        public void Leave() {
            BattleSceneManager.Leave(player);
        }

        public void Resurrected(CombatCharacter character) {
            whoseTurn.Add(character);
        }

        void BuildSpeed() {
            foreach (var nextTurn in whoseTurn)
                nextTurn.NewTurn();
        }

        void HandleTurnAlly(CombatCharacter myTurn) {
            CurrentPlayerControlled = myTurn;
            // Linq to get array of most threatening enemies

            var possibleEnemyTargets = whoseTurn.Where(t => !t.Ally)
                                                .OrderByDescending(t => t.Threat).GroupBy(t => t.Threat)
                                                .SelectMany(g => g).ToArray();

            battleTarget.SetPossibleTargets(possibleEnemyTargets);

            if (battleTarget.EnemyTargeted == null)
                HaveATeamWon();
            else
                battleTarget.EnemyTargeted.Combatant.Target();

            BattleUIManager.Instance.HandleTurnAlly(myTurn);
            waitingForPlayerInput = true;
        }


        IEnumerator HandleTurnEnemy(CombatCharacter myTurn) {
            BattleUIManager.Instance.EnemyTurn();
            // Attack player tea
            var tempList = whoseTurn.FindAll(cc => cc.Ally);
            if (tempList.Count == 0) {
                HaveATeamWon();
                yield break;
            }

            tempList.Sort((cc1, cc2) => cc2.Threat.CompareTo(cc1.Threat));
            var target = tempList[0];

            target.Combatant.Target();
            yield return battleAI.HandleTurn(myTurn, target);
            target.Combatant.StopTargeting();
            NextTurn();
        }

        public void GoToDefeat() => BattleSceneManager.GoToDefeat(player, enemyTeamChars);

        public void GoToAfterBattle() => BattleSceneManager.GoToAfterBattle(player, enemyTeamChars);

        public async Task Hide() {
            SceneLoader.ActionSceneLoaded -= Setup;
            gameObject.SetActive(false);
            await Task.Yield();
        }
    }
}