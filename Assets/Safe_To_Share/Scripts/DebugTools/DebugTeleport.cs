using System;
using System.Collections.Generic;
using AvatarStuff.Holders;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.DebugTools
{
    public class DebugTeleport : MonoBehaviour
    {
        [SerializeField] List<ButtonToPoint> teleportBtns = new();

        // Start is called before the first frame update
        void Start()
        {
            foreach (ButtonToPoint point in teleportBtns)
                point.Setup();
        }

        [Serializable]
        public class ButtonToPoint
        {
            [SerializeField] DebugTeleportPoint point;
            [SerializeField] Button btn;
            public void Setup() => btn.onClick.AddListener(Teleport);
            void Teleport() => PlayerHolder.Instance.transform.position = point.transform.position;
        }
    }
}