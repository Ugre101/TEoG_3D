using System;
using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    public class PrefabTree : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out PlayerHolder player))
            {
                Debug.Log("Trigger enter");
            }
        }
    }
}