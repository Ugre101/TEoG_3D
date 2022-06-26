using System.Collections;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using UnityEngine;

namespace DormAndHome.Dorm
{
    public class DormTester : MonoBehaviour
    {
        [SerializeField] EnemyPreset dormMate;
        DormManager dormManager;

        DormSceneManager dormSceneManager;

        // Start is called before the first frame update
        void Start()
        {
            dormManager = DormManager.Instance;
            dormSceneManager = FindObjectOfType<DormSceneManager>();
        }

        [ContextMenu("Add Mate")]
        void AddMate() => StartCoroutine(AddLoadedMate());

        IEnumerator AddLoadedMate()
        {
            yield return dormMate.LoadAssets();
            Enemy enemy = new(dormMate.NewEnemy());
            dormManager.AddToDorm(new DormMate(enemy));
            dormSceneManager.Loaded();
        }
    }
}