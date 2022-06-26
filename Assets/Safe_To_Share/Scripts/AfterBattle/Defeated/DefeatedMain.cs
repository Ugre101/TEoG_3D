using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.DefeatScenarios.Nodes;
using Character.EnemyStuff;
using Character.PlayerStuff;
using Defeated;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated
{
    public class DefeatedMain : DefeatShared
    {
        LoseScenarioNode currentNode;

        LoseScenario currentScenario;

        protected override void HandleGiveIn()
        {
            currentNode.HandleEffects(activeEnemyActor.Actor, activePlayerActor.Actor);
            UI.PrintNodeEffect(currentNode.GiveInText);
        }

        protected override void HandleResist()
        {
            if (Resistance >= currentNode.ResistCost)
            {
                Resistance -= currentNode.ResistCost;
                UI.Resisted();
                NextNode();
            }
            else
            {
                UI.FailedResist();
                HandleGiveIn();
            }
        }

        protected override void HandleContinue() => NextNode();

        public override void Setup(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies)
        {
            SharedSetup(player, enemies, allies);
            if (activeEnemyActor.Actor is Enemy enemy)
                SetupScenario(enemy.LoseScenarios);
            else
                Leave();
        }

        void SetupScenario(string enemyLoseScenarios)
        {
            if (string.IsNullOrWhiteSpace(enemyLoseScenarios))
            {
                Leave();
                return;
            }

            StartCoroutine(LoadScenario(enemyLoseScenarios));
        }

        IEnumerator LoadScenario(string enemyLoseScenarios)
        {
            var op = Addressables.LoadAssetAsync<LoseScenario>(enemyLoseScenarios);
            yield return op;
            if (op.Status == AsyncOperationStatus.Succeeded && op.Result.GetRootNode() is LoseScenarioNode startNode &&
                startNode != null)
                StartNode(op.Result, startNode);
            else
                Leave();
        }

        void StartNode(LoseScenario scenario, LoseScenarioNode startNode)
        {
            currentScenario = scenario;
            currentNode = startNode;
            UI.StartNode(startNode.IntroText);
        }

        void ShowNode(LoseScenarioNode node)
        {
            currentNode = node;
            UI.SetupNode(node.IntroText);
        }

        void NextNode()
        {
            var nodes = currentScenario.GetChildNodes(currentNode).ToArray();
            if (nodes.Length <= 0)
            {
                UI.ShowLeaveBtn(true);
                return;
            }

            List<LoseScenarioNode> converted = new();
            foreach (BaseEditorCanvasNode node in nodes)
                if (node is LoseScenarioNode loseNode)
                    converted.Add(loseNode);
            converted = converted.FindAll(c => c.CanDo(activeEnemyActor.Actor, activePlayerActor.Actor));
            if (converted.Count <= 0)
            {
                UI.ShowLeaveBtn(true);
                return;
            }

            currentNode = converted[Rng.Next(nodes.Length)];
            if (currentNode == null)
            {
                UI.ShowLeaveBtn(true);
                return;
            }

            switch (currentNode)
            {
                case LoseScenarioDrainEssenceNode loseScenarioDrainEssenceNode:
                    ShowNode(loseScenarioDrainEssenceNode);
                    break;
                case LoseScenarioNodeBodyMorph loseScenarioNodeBodyMorph:
                    ShowNode(loseScenarioNodeBodyMorph);
                    break;
                case LoseScenarioNodeTriesToVore loseScenarioNodeTriesToVore:
                    if (loseScenarioNodeTriesToVore.CanDo(activeEnemyActor.Actor, activePlayerActor.Actor))
                        ShowNode(loseScenarioNodeTriesToVore);
                    else
                        UI.ShowLeaveBtn(true);
                    break;
                case { } basic:
                    ShowNode(basic);
                    break;
            }
        }
    }
}