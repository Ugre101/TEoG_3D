using System.Threading.Tasks;

namespace Character.CreateCharacterStuff {
    public static class EnemyPresetExtensions {
        public static async Task LoadEnemyPresets(this EnemyPreset[] presets) {
            var tasks = new Task[presets.Length];
            for (var i = 0; i < presets.Length; i++)
                tasks[i] = presets[i].LoadAssets();
            await Task.WhenAll(tasks);
        }
    }
}