using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class Spin : MonoBehaviour
    {
        [SerializeField, Range(5f, 15f),] float spinRate = 1f;
        [SerializeField] new Transform transform;
        [SerializeField] Renderer rend;

        void Update()
        {
            if (rend.isVisible)
                transform.RotateAround(transform.position, Vector3.up, spinRate * Time.deltaTime);
        }
    }
}