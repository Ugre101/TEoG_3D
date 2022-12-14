using UnityEditor;

namespace Character.CreateCharacterStuff.EditorPresets
{
    [CustomEditor(typeof(BossPreset))]
    public class BossPresetEditor : EnemyPresetEditor
    {
        void OnEnable()
        {
            BaseOnEnable();
            EnemyOnEnable();
        }
    }
}