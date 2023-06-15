using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.DefeatScenarios.Custom {
    [Serializable]
    public class CustomLoseScenario {
        public string Title = "New Scenario";
        [SerializeField] CustomIntroNode introNode;
        [SerializeField] List<CustomDrainNode> drainNodes = new();
        [SerializeField] List<CustomBodyNode> bodyNodes = new();
        [SerializeField] List<CustomVoreNode> voreNodes = new();
        [SerializeField] List<string> enemies = new();
        Dictionary<string, CustomLoseScenarioNode> childNodesDict;
        public CustomLoseScenario() => introNode = new CustomIntroNode(new Vector2(100, -350));
        public List<string> Enemies => enemies;

        public CustomIntroNode IntroNode => introNode;

        public Dictionary<string, CustomLoseScenarioNode> ChildNodesDict {
            get {
                if (childNodesDict == null) {
                    childNodesDict = new Dictionary<string, CustomLoseScenarioNode> {
                        { introNode.id, introNode },
                    };
                    AddListToDict(drainNodes);
                    AddListToDict(bodyNodes);
                    AddListToDict(voreNodes);

                    void AddListToDict(IEnumerable<CustomLoseScenarioNode> range) {
                        foreach (var node in range)
                            childNodesDict.Add(node.id, node);
                    }
                }

                return childNodesDict;
            }
        }

        public event Action<CustomLoseScenarioNode> NewNode;
        public event Action RemovedNode;

        public IEnumerable<CustomLoseScenarioNode> GetChildNodes(CustomLoseScenarioNode node) {
            foreach (var id in node.childNodesIds)
                if (ChildNodesDict.TryGetValue(id, out var childNode))
                    yield return childNode;
        }


        public string CreateNewNode(CustomNodeTypes nodeType, Vector2 canvasPos) {
            var id = Guid.NewGuid().ToString();
            childNodesDict = null;
            switch (nodeType) {
                case CustomNodeTypes.EssenceDrain:
                    CustomDrainNode drainNode = new(id, canvasPos);
                    drainNodes.Add(drainNode);
                    NewNode?.Invoke(drainNode);
                    break;
                case CustomNodeTypes.BodyMorphs:
                    CustomBodyNode bodyNode = new(id, canvasPos);
                    bodyNodes.Add(bodyNode);
                    NewNode?.Invoke(bodyNode);
                    break;
                case CustomNodeTypes.Vore:
                    CustomVoreNode voreNode = new(id, canvasPos);
                    voreNodes.Add(voreNode);
                    NewNode?.Invoke(voreNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return id;
        }

        public void RemoveNode(CustomLoseScenarioNode node) {
            foreach (var n in ChildNodesDict.Values.Where(n => n.childNodesIds.Contains(node.id)))
                n.childNodesIds.Remove(node.id);
            switch (node) // Could use node.type but I would need to cast anyway to remove
            {
                case CustomDrainNode drainNode:
                    drainNodes.Remove(drainNode);
                    break;
                case CustomBodyNode bodyNode:
                    bodyNodes.Remove(bodyNode);
                    break;
                case CustomVoreNode voreNode:
                    voreNodes.Remove(voreNode);
                    break;
            }

            childNodesDict = null;
            RemovedNode?.Invoke();
        }

        public bool NotLooping(CustomLoseScenarioNode node, string obj) {
            if (!ChildNodesDict.TryGetValue(obj, out var linkTo))
                return false;
            return !linkTo.childNodesIds.Contains(node.id) && !DeeperCheck(linkTo.childNodesIds, node.id);
        }

        bool DeeperCheck(List<string> ids, string nodeId) {
            foreach (var deeperId in ids)
                if (ChildNodesDict.TryGetValue(deeperId, out var deepNode) &&
                    (deepNode.childNodesIds.Contains(nodeId) || DeeperCheck(deepNode.childNodesIds, nodeId)))
                    return true;
            return false;
        }
    }

    public enum CustomNodeTypes {
        EssenceDrain,
        BodyMorphs,
        Vore,
        Intro,
    }
}