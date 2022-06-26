using UnityEditor;

namespace Character.CreateCharacterStuff.EditorPresets
{
    [CustomEditor(typeof(EnemyPreset))]
    public class EnemyPresetEditor : CharacterPresetEditor
    {
        static bool rewardFold, canTakeFold,loseFold;
        SerializedProperty canTake;
        SerializedProperty reward;
        SerializedProperty loseScenario;
        void OnEnable()
        {
            BaseOnEnable();
            reward = serializedObject.FindProperty("battleReward");
            canTake = serializedObject.FindProperty("canTakeEnemyHome");
            loseScenario = serializedObject.FindProperty("loseScenarios");
        }

        protected override void CloseFolds()
        {
            base.CloseFolds();
            rewardFold = false;
            canTakeFold = false;
        }

        public override void OnInspectorGUI()
        {
            Buttons();

            EditorGUILayout.BeginHorizontal();
            FoldButton("Reward", ref rewardFold);
            FoldButton("Take To Dorm", ref canTakeFold);
            FoldButton("Lose Scenario",ref loseFold);
            EditorGUILayout.EndHorizontal();

            PropertyFolds();
            BasicPropertyFold(rewardFold, reward);
            BasicPropertyFold(canTakeFold, canTake);
            BasicPropertyFold(loseFold,loseScenario);
            BottomStuff();
        }

        void BasicPropertyFold(bool fold, SerializedProperty property)
        {
            if (!fold) return;
            EditorGUILayout.BeginVertical("box");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
        }
    }
}