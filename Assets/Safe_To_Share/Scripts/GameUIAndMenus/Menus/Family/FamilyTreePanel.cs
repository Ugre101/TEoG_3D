using System.Collections.Generic;
using Character.Family;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace GameUIAndMenus.Menus.Family
{
    public class FamilyTreePanel : GameMenu
    {
        [SerializeField] ChildTreePrefab prefab;
        [SerializeField] Transform birthed, fathered;

        void OnEnable()
        {
            birthed.KillChildren();
            fathered.KillChildren();
            List<int> children = Player.FamilyTree.Children;
            if (children == null)
                return;
            foreach (int id in children)
                PrintChild(id);
        }

        private void PrintChild(int id)
        {
            if (!DayCare.ChildDict.TryGetValue(id, out Child myChild))
                return;
            if (myChild.FamilyTree.Mother.ID == Player.Identity.ID)
                AddBirthed(myChild);
            else
                AddFathered(myChild);
        }

        void AddFathered(Child myChild) => Instantiate(prefab, fathered).Setup(myChild);

        void AddBirthed(Child myChild) => Instantiate(prefab, birthed).Setup(myChild);
    }
}