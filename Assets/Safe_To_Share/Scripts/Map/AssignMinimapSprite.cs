using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class AssignMinimapSprite : MonoBehaviour
    {
        [SerializeField] Sprite miniMapSprite;
        [SerializeField] Image miniMapImage, bigMapImage;

        void OnValidate()
        {
            if (miniMapImage != null)
                miniMapImage.sprite = miniMapSprite;
            if (bigMapImage != null)
                bigMapImage.sprite = miniMapSprite;
        }
    }
}