using Character.DefeatScenarios.Custom;
using Character.VoreStuff;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI {
    public sealed class VoreNodeOptions : MonoBehaviour {
        [SerializeField] Button oral, cock, unbirth, boobs;
        [SerializeField] Image oralImage, cockImage, unbirthImage, boobsImage;
        CustomVoreNode voreNode;

        public void Setup(CustomVoreNode voreNode) {
            this.voreNode = voreNode;
            ClearBtn(oral, oralImage, VoreType.Oral, ToggleOralVore);
            ClearBtn(cock, cockImage, VoreType.Balls, ToggleCockVore);
            ClearBtn(unbirth, unbirthImage, VoreType.UnBirth, ToggleUnbirth);
            ClearBtn(boobs, boobsImage, VoreType.Breast, ToggleBoobsVore);
        }

        void ClearBtn(Button btn, Image btnImage, VoreType type, UnityAction toggleEffect) {
            btn.onClick.RemoveAllListeners();
            btnImage.color = voreNode.voreTypes.Contains(type) ? Color.green : Color.white;
            btn.onClick.AddListener(toggleEffect);
        }

        void SharedToggle(VoreType type, Image btnImage) {
            if (!voreNode.voreTypes.Remove(type)) {
                voreNode.voreTypes.Add(type);
                btnImage.color = Color.green;
            } else {
                btnImage.color = Color.white;
            }
        }

        void ToggleOralVore() => SharedToggle(VoreType.Oral, oralImage);

        void ToggleCockVore() => SharedToggle(VoreType.Balls, cockImage);
        void ToggleUnbirth() => SharedToggle(VoreType.UnBirth, unbirthImage);
        void ToggleBoobsVore() => SharedToggle(VoreType.Breast, boobsImage);
    }
}