using AvatarStuff.Holders;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Map
{
    public class HideUntilPlayerIsClose : MonoBehaviour
    {
        [SerializeField] float showDist = 100f;
        bool showing;

        void Update()
        {
            if (Time.frameCount % 30 != 0) return;
            CheckIfPlayerIsClose();
        }

        void CheckIfPlayerIsClose()
        {
            if (!showing && Vector3.Distance(PlayerHolder.Instance.transform.position, transform.position) < showDist)
            {
                transform.AwakeChildren();
                showing = true;
            }
            else if (Vector3.Distance(PlayerHolder.Instance.transform.position, transform.position) > showDist)
            {
                transform.SleepChildren();
                showing = false;
            }
        }
    }
}