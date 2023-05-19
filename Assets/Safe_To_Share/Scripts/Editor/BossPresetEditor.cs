using UnityEditor;

namespace Character.CreateCharacterStuff.EditorPresets
{
    [CustomEditor(typeof(BossPreset))]
    public sealed class BossPresetEditor : EnemyPresetEditor
    {
        void OnEnable()
        {
            BaseOnEnable();
            EnemyOnEnable();
        }
    }
}