using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public class ToggleChildGameObject : MonoBehaviour
    {
        [SerializeField] GameObject[] childObjects = Array.Empty<GameObject>();

        public void ToggleChild(GameObject child)
        {
            foreach (var childObject in childObjects)
            {
                childObject.SetActive(childObject == child);
            }
        }
#if UNITY_EDITOR
        void OnValidate()
        {
            if (transform.childCount != childObjects.Length)
            {
                childObjects = new GameObject[transform.childCount];
                for (var i = 0; i < transform.childCount; i++)
                {
                    childObjects[i] = transform.GetChild(i).gameObject;
                }
            }
        }
#endif
    }
}