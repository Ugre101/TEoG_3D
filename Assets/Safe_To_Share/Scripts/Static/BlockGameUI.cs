using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Static
{
    public sealed class BlockGameUI : MonoBehaviour
    {
        void OnEnable()
        {
            GameUIManager.BlockList.Add(this);
        }

        void OnDisable()
        {
            GameUIManager.BlockList.Remove(this);
        }

        void OnDestroy()
        {
            GameUIManager.BlockList.Remove(this);
        }
    }
}