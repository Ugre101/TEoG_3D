using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace GameUIAndMenus
{
    public class HideGameUI : MonoBehaviour
    {
        static bool hidden;
        [SerializeField] GameObject[] expect;

        void Start()
        {
            if (hidden)
                transform.SleepChildren();
            else
                transform.AwakeChildren(expect);
        }

        public void ToggleHide()
        {
            hidden = !hidden;
            Start();
        }
    }
}