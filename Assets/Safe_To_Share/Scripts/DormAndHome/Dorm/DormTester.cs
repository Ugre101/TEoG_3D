using System.Collections;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using UnityEngine;

namespace DormAndHome.Dorm
{
    public sealed class DormTester : MonoBehaviour
    {
        [SerializeField] EnemyPreset dormMate;
        DormManager dormManager;

        DormSceneManager dormSceneManager;

        // Start is called before the first frame update
        void Start()
        {
            dormManager = DormManager.Instance;
            dormSceneManager = FindObjectOfType<DormSceneManager>(true);
        }

        [ContextMenu("Add Mate")]
        void AddMate() => AddLoadedMate();

        async void AddLoadedMate()
        {
            await dormMate.LoadAssets();
            Enemy enemy = new(dormMate.NewEnemy());
            dormManager.AddToDorm(new DormMate(enemy));
            if (dormSceneManager != null)
                dormSceneManager.Loaded();
        }
    }
}