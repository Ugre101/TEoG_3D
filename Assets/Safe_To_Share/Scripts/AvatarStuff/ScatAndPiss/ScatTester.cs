using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public sealed class ScatTester : MonoBehaviour
    {
        [SerializeField] ScatPrefab scatPrefab;

        [ContextMenu("Test Scat")]
        public void TestScat()
        {
            Instantiate(scatPrefab,transform);
        }
    }
}