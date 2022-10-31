using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class ScatHandler : MonoBehaviour
    {
        [SerializeField] ScatPrefab prefab;
        [SerializeField] GameObject pissPrefab;
        public void Scat(int size)
        {
            ScatPrefab scat = Instantiate(prefab, transform);
        }

        public void Piss(float pressure)
        {
            
        }
    }
}