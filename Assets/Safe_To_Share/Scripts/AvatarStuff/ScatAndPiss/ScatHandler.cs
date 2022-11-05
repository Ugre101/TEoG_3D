using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class ScatHandler : MonoBehaviour
    {
        [SerializeField] ScatPrefab prefab;
        [SerializeField] PissHole pissHole;
        [SerializeField] float zSpawnOffset = 0.01f;
#if UNITY_EDITOR
        [ContextMenu("Test Scat")]
        public void TestScat()
        {
            Scat();
        }
#endif

        public void Scat(float size = 1)
        {
            var scat = Instantiate(prefab, transform.position + new Vector3(0, 0, zSpawnOffset), Quaternion.identity);
        }

        public void Piss(float pressure)
        {
        }
    }
}