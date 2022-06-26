using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.IslandData
{
    public static class IslandStonesDatas
    {
        public static IslandData Village = new();

        static Dictionary<Islands, IslandData> islandDataDict;

        public static Dictionary<Islands, IslandData> IslandDataDict => islandDataDict ??=
            new Dictionary<Islands, IslandData>
            {
                { Islands.Village, Village },
            };

        public static string Save() => JsonUtility.ToJson(new IslandsDataSave(Village));

        public static void Load(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;
            var toLoad = JsonUtility.FromJson<IslandsDataSave>(json);
            Village = toLoad.village.data;
            islandDataDict = null;
        }

        [Serializable]
        public struct IslandsDataSave
        {
            public IslandDataSave village;

            public IslandsDataSave(IslandData village) => this.village = new IslandDataSave(village);

            [Serializable]
            public struct IslandDataSave
            {
                public IslandData data;

                public IslandDataSave(IslandData save) => data = save;
            }
        }
    }
}