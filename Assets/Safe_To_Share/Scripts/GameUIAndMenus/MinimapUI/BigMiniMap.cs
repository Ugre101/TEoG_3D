using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.MinimapUI
{
    public class BigMiniMap : MapShared
    {
        [SerializeField] Transform leftContainer, rightContainer;
        [SerializeField] BigMapNamedIcon prefab;

        protected override void Start()
        {
            base.Start();
            GetBigMapNamedIcons();
        }

        void GetBigMapNamedIcons()
        {
            AddNamedIcons(PairedStaticObjects);
            AddNamedIcons(PairedObjects);
            //AddNamedIcons(enemyZone);
        }

        void AddNamedIcons(List<PairedObject> statics)
        {
            foreach (var miniMapObject in statics)
            {
                var miniMapBaseObject = miniMapObject.GlobalTrans;
                if (miniMapBaseObject.AddIconToBigMap)
                    Instantiate(prefab, ContainerWithLessChildren())
                        .Setup(miniMapBaseObject.AddedIconText, miniMapBaseObject.Icon);
            }
        }

        Transform ContainerWithLessChildren() =>
            rightContainer.childCount >= leftContainer.childCount ? leftContainer : rightContainer;
    }
}