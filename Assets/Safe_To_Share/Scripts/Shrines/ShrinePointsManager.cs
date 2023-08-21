using UnityEngine;

namespace Safe_To_Share.Scripts.Shrines {
    public static class ShrinePointsManager {
        public static ShrinePoints ChimeraShrine { get; private set; } = new();

        public static ShrineSave Save() => new(ChimeraShrine);

        public static void Load(ShrineSave toLoad) {
            ChimeraShrine = JsonUtility.FromJson<ShrinePoints>(toLoad.ChimeraShrine);
        }
    }
}