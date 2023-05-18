using DormAndHome.Dorm;
using SceneStuff;

namespace Safe_To_Share.Scripts
{
    public sealed class LoadDungeonIfOwned : LoadSubSceneWhenClose
    {
        static bool ownsDungeon;

        protected override void Start()
        {
            base.Start();
            CheckOwnsDungeon();
            DormManager.Instance.Buildings.Dungeon.Upgraded += CheckOwnsDungeon;
        }


        protected override void Update()
        {
            if (ownsDungeon)
                base.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DormManager.Instance.Buildings.Dungeon.Upgraded -= CheckOwnsDungeon;
        }

        protected override void LoadedMe()
        {
            base.LoadedMe();
            CheckOwnsDungeon();
            if (!ownsDungeon && subScene.SceneActive && subScene.SceneLoaded)
                UnLoadScene();
        }

        public static void CheckOwnsDungeon() => ownsDungeon = DormManager.Instance.Buildings.Dungeon.Level > 0;
    }
}