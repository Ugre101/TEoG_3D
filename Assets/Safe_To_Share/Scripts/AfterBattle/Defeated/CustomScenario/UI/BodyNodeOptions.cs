using Character.BodyStuff;
using Character.DefeatScenarios.Custom;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI
{
    public sealed class BodyNodeOptions : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown bodyType;
        [SerializeField] Slider permChange;
        [SerializeField] TextMeshProUGUI permChangeText;
        [SerializeField] Button addTempMod;
        [SerializeField] Transform container;
        [SerializeField] TempModButton tempModButton;
        [SerializeField] Toggle stealPermChange;
        CustomBodyNode bodyNode;

        void Start() => addTempMod.onClick.AddListener(AddTempMod);

        void AddTempMod()
        {
            if (bodyNode == null)
                return;
            MakeTempMod mod = new();
            bodyNode.tempMods.Add(mod);
            Instantiate(tempModButton, container).Setup(bodyNode, mod);
        }

        public void Setup(CustomBodyNode bodyNode)
        {
            this.bodyNode = bodyNode;
            container.KillChildren();
            bodyType.SetupTmpDropDown(bodyNode.bodyType, ChangeBodyType);

            permChange.onValueChanged.RemoveAllListeners();
            permChange.value = bodyNode.permChange;
            permChangeText.text = bodyNode.permChange.ToString();
            permChange.onValueChanged.AddListener(PerkChange);

            stealPermChange.onValueChanged.RemoveAllListeners();
            stealPermChange.isOn = bodyNode.transfer;
            stealPermChange.onValueChanged.AddListener(ChangeStealPerm);
        }

        void ChangeStealPerm(bool arg0)
        {
        }

        void PerkChange(float arg0)
        {
            bodyNode.permChange = Mathf.RoundToInt(arg0);
            permChangeText.text = bodyNode.permChange.ToString();
        }

        void ChangeBodyType(int arg0) => bodyNode.bodyType = UgreTools.IntToEnum(arg0, BodyStatType.Height);
    }
}