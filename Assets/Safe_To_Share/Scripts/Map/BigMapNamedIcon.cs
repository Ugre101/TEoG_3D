using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class BigMapNamedIcon : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Image image;

        public void Setup(string text, Sprite sprite)
        {
            title.text = text;
            image.sprite = sprite;
        }
    }
}