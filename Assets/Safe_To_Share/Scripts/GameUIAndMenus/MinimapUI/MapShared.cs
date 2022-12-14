using System.Collections.Generic;
using Map;
using QuestStuff;
using Safe_To_Share.Scripts.Map;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameUIAndMenus.MinimapUI
{
    public abstract class MapShared : GameMenu
    {
        [SerializeField] protected RectTransform map;
        [SerializeField] protected Image mapIconPrefab;
        [SerializeField] protected Image enemyZonePrefab;
        protected readonly List<PairedObject> PairedEnemyZones = new();
        protected readonly List<PairedObject> PairedObjects = new();
        protected readonly List<PairedObject> PairedStaticObjects = new();
        protected readonly List<PairedObject> PairedStaticQuestObject = new();
        Vector2 center;
        bool started;
        float xRadius;
        float zRadius;

        protected virtual void Start()
        {
            Setup();
            started = true;
        }

        protected virtual void OnEnable()
        {
            StaticQuestObject.PrintMe += AddStaticQuestObject;
            StaticQuestObject.RemoveMe += RemoveStaticQuestMarker;
            if (started)
                FindAndInstanceQuests();
        }

        protected virtual void OnDisable()
        {
            StaticQuestObject.PrintMe -= AddStaticQuestObject;
            StaticQuestObject.RemoveMe -= RemoveStaticQuestMarker;
        }

        protected void MoveDynamic(PairedObject pairedObject)
        {
            pairedObject.MiniMapTrans.localPosition = SetMiniMapPos(pairedObject.GlobalTrans.Pos);
            pairedObject.MiniMapTrans.eulerAngles =
                new Vector3(0, 0, -pairedObject.GlobalTrans.transform.eulerAngles.y);
        }

        protected PairedObject InstanceStaticObject(MiniMapBaseObject staticMarker)
        {
            Image obj = GetNewImage(staticMarker);
            PairedObject questObject = new(obj.transform, staticMarker);
            MoveStaticPos(questObject);

            return questObject;
        }

        Image GetNewImage(MiniMapBaseObject staticMarker)
        {
            Image obj = Instantiate(mapIconPrefab, map);
            obj.sprite = staticMarker.Icon;
            SetMiniMapObjectSize(obj);
            return obj;
        }

        protected void SetMiniMapObjectSize(Graphic obj)
        {
            float size = map.rect.width / 10f;
            obj.rectTransform.sizeDelta = new Vector2(size, size);
        }

        protected void RemoveMovingObject(Object dynamicMiniMapObject)
        {
            if (!PairedObjects.Exists(o => o.GlobalTrans == dynamicMiniMapObject))
                return;
            PairedObject obj = PairedObjects.Find(o => o.GlobalTrans == dynamicMiniMapObject);
            PairedObjects.Remove(obj);
            Destroy(obj.MiniMapTrans.gameObject);
        }

        public void RemoveStaticQuestMarker(StaticQuestObject staticStaticQuest)
        {
            if (PairedStaticQuestObject.Find(q => q.GlobalTrans.GetInstanceID() == staticStaticQuest.GetInstanceID()) is { } fund)
            {
                PairedStaticQuestObject.Remove(fund);
                Destroy(fund.MiniMapTrans.gameObject);
            }
        }

        void AddStaticObject(MiniMapBaseObject staticMiniMapObject) =>
            PairedStaticObjects.Add(InstanceStaticObject(staticMiniMapObject));

        void AddMovingObject(MiniMapBaseObject dynamicMiniMapObject) =>
            PairedObjects.Add(InstanceStaticObject(dynamicMiniMapObject));

        void AddStaticQuestObject(StaticQuestObject staticMarker) =>
            PairedStaticQuestObject.Add(InstanceStaticObject(staticMarker));

        protected void MoveStaticPos(PairedObject staticObject)
            => staticObject.MiniMapTrans.localPosition = SetMiniMapPos(staticObject.GlobalTrans.Pos);

        void FindAndInstanceQuests()
        {
            for (int index = PairedStaticQuestObject.Count; index-- > 0;)
            {
                PairedObject pairedObject = PairedStaticQuestObject[index];
                Destroy(pairedObject.MiniMapTrans.gameObject);
                PairedStaticQuestObject.Remove(pairedObject);
            }

            foreach (StaticQuestObject staticQuestObject in FindObjectsOfType<StaticQuestObject>())
                if (staticQuestObject.HasQuest)
                    AddStaticQuestObject(staticQuestObject);
        }

        static float RectOffSet(float f, float axis, float radius) => f / 2 * (axis / radius);

        Vector2 SetMiniMapPos(Vector3 position)
        {
            Vector2 inVec2 = new(position.x, position.z);
            Vector2 step1 = inVec2 - center;

            var rect = map.rect;
            float xOffset = RectOffSet(rect.width, step1.x, xRadius);
            float zOffset = RectOffSet(rect.height, step1.y, zRadius);
            return new Vector2(xOffset, zOffset);
        }

        void AddEnemyZone(EnemyZoneMiniMapObject zone)
        {
            Image obj = Instantiate(enemyZonePrefab, map);
            obj.color = zone.Color;
            PairedObject zoneObject = new(obj.transform, zone);
            MoveStaticPos(zoneObject);
            PairedEnemyZones.Add(zoneObject);
        }

        void Setup()
        {
            if (MapData.Instance == null)
            {
                Debug.LogWarning("No mapData"); 
                return;
            }

            var terrainDataSize = MapData.Instance.MapSize;
            xRadius = terrainDataSize.x / 2;
            zRadius = terrainDataSize.y / 2;
            Vector3 terrainPos = MapData.Instance.MapPosition;
            center = new Vector2(terrainPos.x + xRadius, terrainPos.z + zRadius);
            FindAllAndInstance();
            FindAndInstanceQuests();
        }

        void FindAllAndInstance()
        {
            foreach (StaticMiniMapObject staticMiniMapObject in MapData.Instance.Statics)
                AddStaticObject(staticMiniMapObject);
            foreach (DynamicMiniMapObject mapObject in FindObjectsOfType<DynamicMiniMapObject>(true))
                AddMovingObject(mapObject);
            foreach (EnemyZoneMiniMapObject zone in MapData.Instance.enemyZones)
                AddEnemyZone(zone);
        }

        void CheckQuests()
        {
            for (int index = PairedStaticQuestObject.Count; index-- > 0;)
            {
                PairedObject pairedObject = PairedStaticQuestObject[index];
                if (pairedObject.GlobalTrans is StaticQuestObject { HasQuest: false, })
                {
                    PairedStaticQuestObject.Remove(pairedObject);
                    Destroy(pairedObject.MiniMapTrans.gameObject);
                }
            }
        }

        protected readonly struct PairedObject
        {
            public Transform MiniMapTrans { get; }
            public MiniMapBaseObject GlobalTrans { get; }

            public PairedObject(Transform miniMapTrans, MiniMapBaseObject globalTrans)
            {
                MiniMapTrans = miniMapTrans;
                GlobalTrans = globalTrans;
            }
        }
    }
}