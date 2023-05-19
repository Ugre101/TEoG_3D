using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public sealed class ScatHandler : MonoBehaviour
    {
        [SerializeField] ScatPrefab prefab;
        [SerializeField] float zSpawnOffset = 0.01f;
        [SerializeField] float ySpawnOffset = -0.075f;
#if UNITY_EDITOR
        [ContextMenu("Test Scat")]
        public void TestScat()
        {
            Scat();
        }
#endif

        public void Scat(float size = 1)
        {
            var scat = Instantiate(prefab, transform.position + new Vector3(0, ySpawnOffset, zSpawnOffset), Quaternion.identity);
            scat.transform.localScale *= size;
        }
    }
}