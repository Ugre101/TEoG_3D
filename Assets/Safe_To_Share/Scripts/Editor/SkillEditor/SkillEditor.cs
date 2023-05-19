using Battle.SkillsAndSpells;
using EffectStuff.Editor;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using UnityEditor;

namespace Character.SkillsAndSpells.Editor
{
    [CustomEditor(typeof(Ability))]
    public class AbilityBaseEditor : UnityEditor.Editor
    {
        SerializedProperty abilityTree;
        bool effectTreeFoldOut;
        Ability myTarget;

        SerializedProperty showEffect;

        void OnEnable()
        {
            myTarget = (Ability)target;
            abilityTree = serializedObject.FindProperty("effectsTree");
        }

        public override void OnInspectorGUI()
        {
            myTarget = (Ability)target;

            serializedObject.Update();
            EffectEditor.PaintEffectTree(abilityTree, ref showEffect);
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(Skill))]
    public sealed class SkillEditor : AbilityBaseEditor
    {
    }

    [CustomEditor(typeof(Spell))]
    public sealed class SpellEditor : AbilityBaseEditor
    {
    }

    [CustomEditor(typeof(Surrender))]
    public sealed class SurrenderEditor : AbilityBaseEditor
    {
    }
}