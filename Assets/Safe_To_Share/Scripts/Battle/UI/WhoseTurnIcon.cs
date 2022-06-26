using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class WhoseTurnIcon : MonoBehaviour
    {
        [SerializeField] Image background;
        [SerializeField] TextMeshProUGUI title;
        public void Setup(bool ally,string firstName)
        {
            background.color = ally ? Color.blue : Color.red;
            title.text = firstName;
        }
    }
}