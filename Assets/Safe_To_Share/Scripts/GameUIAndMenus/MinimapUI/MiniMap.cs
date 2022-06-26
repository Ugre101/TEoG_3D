using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.MinimapUI
{
    public class MiniMap : MapShared
    {
        [SerializeField] RunTimeReSizeRectTransform sizeChanger;

        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {
            if (FrameLimit())
                return;
            foreach (PairedObject pairedObject in PairedObjects)
                MoveDynamic(pairedObject);
        }

        protected override void OnEnable()
        {
            sizeChanger.ChangedSize += SetPosAll;
            sizeChanger.ChangedSize += ReSizeAll;
            base.OnEnable();
        }


        protected override void OnDisable()
        {
            sizeChanger.ChangedSize -= SetPosAll;
            sizeChanger.ChangedSize -= ReSizeAll;
            base.OnDisable();
        }


        public override bool BlockIfActive() => false;

        static bool FrameLimit() => Time.frameCount % 6 != 0;

        void ReSizeAll()
        {
            ReSize(PairedObjects);
            ReSize(PairedEnemyZones);
            ReSize(PairedStaticObjects);
            ReSize(PairedStaticQuestObject);

            void ReSize(IEnumerable<PairedObject> objects)
            {
                foreach (PairedObject pairedObject in objects)
                    if (pairedObject.GlobalTrans.TryGetComponent(out Graphic graphic))
                        SetMiniMapObjectSize(graphic);
            }
        }


        void SetPosAll()
        {
            foreach (PairedObject staticObject in PairedStaticObjects)
                MoveStaticPos(staticObject);
            foreach (PairedObject pairedObject in PairedObjects)
                MoveDynamic(pairedObject);
            foreach (PairedObject staticQuestObject in PairedStaticQuestObject)
                MoveStaticPos(staticQuestObject);
            foreach (PairedObject zone in PairedEnemyZones)
                MoveStaticPos(zone);
        }
    }
}