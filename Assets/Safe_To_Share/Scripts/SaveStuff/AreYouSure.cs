using System;
using UnityEngine;
using UnityEngine.UI;

namespace SaveStuff
{
    public class AreYouSure : MonoBehaviour
    {
        [SerializeField] Button acceptBtn, declineBtn;
        Action action;

        void Start()
        {
            acceptBtn.onClick.AddListener(Accept);
            declineBtn.onClick.AddListener(Decline);
        }

        public void Setup(Action acceptAction)
        {
            gameObject.SetActive(true);
            action = acceptAction;
        }

        void Accept()
        {
            action?.Invoke();
            gameObject.SetActive(false);
        }

        void Decline()
        {
            action = null;
            gameObject.SetActive(false);
        }
    }
}