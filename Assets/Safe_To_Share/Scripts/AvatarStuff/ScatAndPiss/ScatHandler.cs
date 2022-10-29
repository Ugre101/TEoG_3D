using UnityEngine;

namespace AvatarStuff
{
    public class ScatHandler : MonoBehaviour
    {
        [SerializeField] ScatPrefab prefab;
        [SerializeField] GameObject pissPrefab;
        public void Scat(int size)
        {
            ScatPrefab scat = Instantiate(prefab, transform);
            scat.Play();
        }

        public void Piss(float pressure)
        {
            
        }
    }
}