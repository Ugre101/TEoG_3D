using UnityEngine;
using UnityEngine.EventSystems;

namespace Safe_To_Share.Scripts
{
    public sealed class HoverOverHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject texT;

        // Start is called before the first frame update
        void Start() => texT.SetActive(false);


        public void OnPointerEnter(PointerEventData eventData) => texT.SetActive(true);

        public void OnPointerExit(PointerEventData eventData) => texT.SetActive(false);
    }
}