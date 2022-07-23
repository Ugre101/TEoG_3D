using System;
using UnityEngine;

namespace Character.RelationShipStuff
{
    [Serializable]
    public class RelationShip
    {
        [SerializeField] int withID;
        [SerializeField] float affection;
        [SerializeField] float subDom;

        public RelationShip(int withID, float affection = 0, float subDom = 0)
        {
            this.withID = withID;
            this.affection = affection;
            this.subDom = subDom;
        }

        public float Submission
        {
            get => Mathf.RoundToInt(subDom);
            set => subDom = value;
        }

        public float Affection
        {
            get => Mathf.RoundToInt(affection);
            set => affection = value;
        }


        public int WithID => withID;
    }
}