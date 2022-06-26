using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    public class HeightLimitArea : MonoBehaviour
    {
        [SerializeField] float heightLimit = 1f;

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            holder.Scaler.AreaHasHeightLimit(heightLimit);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            holder.Scaler.ExitHeightLimitArea();
        }
    }
}
