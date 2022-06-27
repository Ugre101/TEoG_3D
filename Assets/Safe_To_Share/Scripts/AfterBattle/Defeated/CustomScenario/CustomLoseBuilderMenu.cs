using System.IO;
using Character.DefeatScenarios.Custom;
using Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario
{
    public class CustomLoseBuilderMenu : MonoBehaviour
    {
        [SerializeField] Button newScenario, saveScenario;
        [SerializeField] SelectedNode showSelected;
        [SerializeField] NodeUI prefab;
        [SerializeField] Transform container;
        [SerializeField] TMP_InputField titleInput;
        public CustomLoseScenario loseScenario;
        DirectoryInfo modsFolder;
        static string ModsFolderPath => Path.Combine(Application.dataPath, "Mods");
        public static string CustomScenarioFolder => Path.Combine(ModsFolderPath, "CustomLoseScenarios");

        public void Start()
        {
            modsFolder = Directory.CreateDirectory(ModsFolderPath);
            newScenario.onClick.AddListener(NewScenario);
            saveScenario.onClick.AddListener(SaveScenarioToModsFolder);
            NewScenario();
            titleInput.text = loseScenario.Title;
            titleInput.onValueChanged.AddListener(NewTitle);
            LoseScenarioLoadMenuButton.LoadScenario += LoadScenario;
        }

        void LoadScenario(CustomLoseScenario obj) => AddScenario(obj);

        void NewTitle(string arg0)
        {
            if (loseScenario == null)
                return;
            loseScenario.Title = arg0;
        }

        void SaveScenarioToModsFolder()
        {
            if (loseScenario == null)
                return;
            string savePath = CustomScenarioFolder;
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            savePath = Path.Combine(savePath, loseScenario.Title);
            while (File.Exists($"{savePath}.json")) savePath += "new";
            savePath += ".json";
            File.WriteAllText(savePath, JsonUtility.ToJson(loseScenario));
        }

        void NewScenario() => AddScenario(new CustomLoseScenario());

        void AddScenario(CustomLoseScenario scenario)
        {
            if (loseScenario != null)
            {
                loseScenario.NewNode -= PrintNode;
                loseScenario.RemovedNode -= RemovedNode;
                loseScenario.RemovedNode -= PrintLinks;
            }

            loseScenario = scenario;
            container.KillChildren();
            foreach (var n in loseScenario.ChildNodesDict.Values)
                PrintNode(n);
            loseScenario.NewNode += PrintNode;
            loseScenario.RemovedNode += RemovedNode;
            loseScenario.RemovedNode += PrintLinks;
            showSelected.Setup(loseScenario.IntroNode);
        }

        void PrintLinks(CustomLoseScenarioNode throwAway) => PrintLinks();

        void PrintLinks()
        {
            var nodes = container.GetComponentsInChildren<NodeUI>();
            foreach (var addedNode in nodes)
                addedNode.ShowLinks();
        }

        void RemovedNode()
        {
        }

        void PrintNode(CustomLoseScenarioNode node)
        {
            var newNode = Instantiate(prefab, container);
            newNode.transform.localPosition = node.canvasPos;
            newNode.SetupNode(loseScenario, node);
            newNode.ShowMe += showSelected.Setup;
            newNode.ShowLinks();
        }
    }
}