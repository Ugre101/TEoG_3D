using System.Linq;
using Character;
using Character.DefeatScenarios.Custom;
using Character.EnemyStuff;
using Character.PlayerStuff;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated {
    public sealed class CustomDefeatedMain : DefeatShared {
        CustomLoseScenarioNode currentNode;

        CustomLoseScenario currentScenario;

        protected override void HandleGiveIn() {
            currentNode.HandleEffects(activeEnemyActor.Actor, activePlayerActor.Actor);
            UI.PrintNodeEffect(currentNode.giveInText);
        }

        protected override void HandleResist() {
            if (Resistance >= currentNode.resistCost) {
                Resistance -= currentNode.resistCost;
                UI.Resisted();
                NextNode();
            } else {
                UI.FailedResist();
                HandleGiveIn();
            }
        }

        protected override void HandleContinue() => NextNode();

        public override void Setup(Player player, BaseCharacter[] enemies, BaseCharacter[] allies = null) {
            SharedSetup(player, enemies, allies);
            if (activeEnemyActor.Actor is Enemy enemy && enemy.CustomLoseScenarios.Count > 0)
                SetupScenario(enemy.CustomLoseScenarios[Rng.Next(enemy.CustomLoseScenarios.Count)]);
            else
                Leave();
        }

        void SetupScenario(CustomLoseScenario customLose) {
            if (customLose?.IntroNode == null)
                Leave();
            else
                StartNode(customLose, customLose.IntroNode);
        }

        void StartNode(CustomLoseScenario scenario, CustomLoseScenarioNode startNode) {
            currentScenario = scenario;
            currentNode = startNode;
            UI.StartNode(startNode.introText);
        }

        void ShowNode(CustomLoseScenarioNode node) {
            currentNode = node;
            UI.SetupNode(node.introText);
        }

        void NextNode() {
            var nodes = currentScenario.GetChildNodes(currentNode).ToArray();
            if (nodes.Length <= 0) {
                UI.ShowLeaveBtn(true);
                return;
            }

            var converted = nodes.Where(n => n.CanDo(activeEnemyActor.Actor, activePlayerActor.Actor)).ToArray();
            if (converted.Length <= 0) {
                UI.ShowLeaveBtn(true);
                return;
            }

            currentNode = converted[Rng.Next(converted.Length)];
            if (currentNode == null) {
                UI.ShowLeaveBtn(true);
                return;
            }

            switch (currentNode) {
                case CustomDrainNode loseScenarioDrainEssenceNode:
                    ShowNode(loseScenarioDrainEssenceNode);
                    break;
                case CustomBodyNode loseScenarioNodeBodyMorph:
                    ShowNode(loseScenarioNodeBodyMorph);
                    break;
                case CustomVoreNode loseScenarioNodeTriesToVore:
                    if (loseScenarioNodeTriesToVore.CanDo(activeEnemyActor.Actor, activePlayerActor.Actor))
                        ShowNode(loseScenarioNodeTriesToVore);
                    else
                        UI.ShowLeaveBtn(true);
                    break;
                case { } intro:
                    ShowNode(intro);
                    break;
            }
        }
    }
}