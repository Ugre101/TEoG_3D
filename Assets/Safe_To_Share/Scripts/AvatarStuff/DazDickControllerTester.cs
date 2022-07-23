using UnityEngine;

namespace AvatarStuff
{
    public class DazDickControllerTester : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f),] float boner;
        [SerializeField, Range(0.1f, 20f),] float size = 1f;
        [SerializeField] bool hidden;

        void OnValidate()
        {
            if (Application.isPlaying) return;
            if (!TryGetComponent(out DazDickController component)) return;
            component.SetBoner(boner);
            component.SetDickSize(size);
            component.HideOrShow(!hidden);
        }
    }
}