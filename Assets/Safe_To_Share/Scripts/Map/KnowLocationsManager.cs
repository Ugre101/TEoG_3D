using System;
using System.Collections.Generic;
using System.Linq;
using SaveStuff;
using SceneStuff;
using UnityEngine;

namespace Map
{
    public static class KnowLocationsManager
    {
        static Dictionary<string, Location> dict;
        public static List<Location> KnownLocations { get; private set; } = new();
        public static Dictionary<string, Location> Dict => dict ??= KnownLocations.ToDictionary(l => l.LocationGuid);


        public static void LearnLocation(LocationSceneSo locationSceneSo, params string[] exit) =>
            LearnLocation(locationSceneSo.Guid, exit);

        public static void LearnLocation(string locationSceneSoGuid, params string[] exit)
        {
            if (Dict.ContainsKey(locationSceneSoGuid))
                return;
            KnownLocations.Add(new Location(locationSceneSoGuid, new List<string>(exit)));
            dict = null;
        }

        public static KnowLocationsSave Save() => new(KnownLocations);

        public static void Load(KnowLocationsSave toLoad)
        {
            KnownLocations = toLoad.LocationsList;
            dict = null;
        }

        [Serializable]
        public struct Location
        {
            [SerializeField] string locationGuid;
            [SerializeField] List<string> exitsGuids;

            public Location(string locationGuid, List<string> exitsGuids)
            {
                this.locationGuid = locationGuid;
                this.exitsGuids = exitsGuids;
            }

            public string LocationGuid => locationGuid;
            public List<string> ExitsGuids => exitsGuids;

            public void LearnExit(string exitGuid)
            {
                if (exitsGuids.Contains(exitGuid))
                    return;
                exitsGuids.Add(exitGuid);
            }
        }
    }

    [Serializable]
    public struct KnowLocationsSave
    {
        [SerializeField] List<KnowLocationsManager.Location> locationsList;

        public KnowLocationsSave(List<KnowLocationsManager.Location> locationsList) =>
            this.locationsList = locationsList;

        public List<KnowLocationsManager.Location> LocationsList => locationsList;
    }
}