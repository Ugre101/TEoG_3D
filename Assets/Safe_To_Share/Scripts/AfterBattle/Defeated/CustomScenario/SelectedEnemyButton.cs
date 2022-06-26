using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario
{
    public class SelectedEnemyButton : MonoBehaviour
    {
        public Button btn;
        [SerializeField] TextMeshProUGUI buttonText, enemyDesc;
        [SerializeField] Image image;

        public void Setup(string title, string desc)
        {
            buttonText.text = title;
            enemyDesc.text = desc;
        }

        public void SetColor(Color color) => image.color = color;
    }
}