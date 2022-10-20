using UnityEngine;

namespace AvatarStuff
{
    public class ScatHandler : MonoBehaviour
    {
        [SerializeField] ScatPrefab prefab;

        public void Scat(int size)
        {
            ScatPrefab scat = Instantiate(prefab, transform);
            scat.Play();
        }
    }
}