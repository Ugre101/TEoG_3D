using System;
using System.Collections.Generic;
using Character.IdentityStuff;
using UnityEngine;

namespace Character.Family {
    [Serializable]
    public class FamilyTree {
        [SerializeField] Identity father;
        [SerializeField] Identity mother;
        [SerializeField] List<int> childrenIds;

        public FamilyTree() {
            father = new Identity();
            mother = new Identity();
            childrenIds = new List<int>();
        }

        public FamilyTree(Identity father, Identity mother) {
            this.father = father;
            this.mother = mother;
            childrenIds = new List<int>();
        }

        public Identity Father => father;

        public Identity Mother => mother;

        public List<int> Children => childrenIds;
    }
}