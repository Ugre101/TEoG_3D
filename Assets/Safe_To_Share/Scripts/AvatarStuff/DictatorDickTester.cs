using UnityEngine;

namespace AvatarStuff
{
    public class DictatorDickTester : MonoBehaviour
    {
        [SerializeField] DictatorDick dick;
        [SerializeField, Range(0, 10f),] float size;
        [SerializeField] bool hidden;
        void OnValidate()
        {
            if (dick == null)
                if (TryGetComponent(out DictatorDick foundDick))
                    dick = foundDick;
                else
                    return;
            dick.SetDickSize(size);
            dick.HideOrShow(!hidden);
        }
    }
}