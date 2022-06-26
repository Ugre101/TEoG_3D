using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.RelationShipStuff
{
    [Serializable]
    public class RelationsShips
    {
        [SerializeField] List<RelationShip> relationShips = new();

        public List<RelationShip> RelationShips => relationShips;

        Dictionary<int, RelationShip> relationShipsDict;

        public Dictionary<int, RelationShip> RelationShipsDict => relationShipsDict ??= RelationShips.ToDictionary(r => r.WithID);
        public RelationShip GetRelationShipWith(BaseCharacter character) => GetRelationShipWith(character.Identity.ID);
        public RelationShip GetRelationShipWith(int id)
        {
            if (RelationShipsDict.TryGetValue(id, out RelationShip relation))
                return relation;
            RelationShip newRelation = new(id);
            relationShips.Add(newRelation);
            relationShipsDict = null;
            return newRelation;
        }

        public void IncreaseAffectionTowards(BaseCharacter towards, float by) => GetRelationShipWith(towards.Identity.ID).Affection += by;

        public void DecreaseAffectionTowards(BaseCharacter towards, float by) => GetRelationShipWith(towards.Identity.ID).Affection -= by;
        public void IncreaseSubmissivenessTowards(int id, float by) => GetRelationShipWith(id).Affection += by;
        public void IncreaseSubmissivenessTowards(BaseCharacter towards, float by) => IncreaseSubmissivenessTowards(towards.Identity.ID, by);
        public void IncreaseDomesivenessTowards(BaseCharacter towards, float by) => GetRelationShipWith(towards.Identity.ID).Submission -= by;
    }
}