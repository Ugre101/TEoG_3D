using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SaveStuff
{
    public class NewSaveField : MonoBehaviour
    {
        [SerializeField] TMP_InputField saveName;
        [SerializeField] Button saveBtn;
        [SerializeField] InputActionAsset actionMap;
        [SerializeField] InputAction saveHotKey = new();

        void Start()
        {
            saveBtn.onClick.AddListener(SaveGame);
            if (actionMap == null)
                return;
            saveName.onSelect.AddListener(DisableMap);
            saveName.onDeselect.AddListener(EnableMap);
            saveName.onSubmit.AddListener(EnableMap);
            //    saveName.onEndEdit.AddListener(EnableMap);

            saveHotKey.performed += context => SaveGame();
        }

        void OnDisable() => EnableMap("Throw away");

        public static event Action<string> NewSave;

        void EnableMap(string arg0)
        {
            saveHotKey.Disable();
            actionMap.Enable();
        }

        void DisableMap(string arg0)
        {
            saveHotKey.Enable();
            actionMap.Disable();
        }


        void SaveGame() => NewSave?.Invoke(string.IsNullOrEmpty(saveName.text) ? "New Save" : saveName.text);
    }
}