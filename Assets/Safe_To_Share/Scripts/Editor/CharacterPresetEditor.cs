using UnityEditor;
using UnityEngine;

namespace Character.CreateCharacterStuff.EditorPresets {
    [CustomEditor(typeof(CharacterPreset))]
    public class CharacterPresetEditor : Editor {
        static bool genderFold, identityFold, statsFold, raceFold, bodyFold;

        static bool baseEditorFold;
        SerializedProperty startBody;
        SerializedProperty startGender;
        SerializedProperty startIdentity;
        SerializedProperty startRace;
        SerializedProperty startStats;

        void OnEnable() => BaseOnEnable();

        protected void BaseOnEnable() {
            startGender = serializedObject.FindProperty("startGender");
            startIdentity = serializedObject.FindProperty("startIdentity");
            startStats = serializedObject.FindProperty("startStats");
            startRace = serializedObject.FindProperty("startRaceRef");
            startBody = serializedObject.FindProperty("startBody");
        }

        protected virtual void CloseFolds() {
            genderFold = false;
            identityFold = false;
            statsFold = false;
            raceFold = false;
            bodyFold = false;
        }

        public override void OnInspectorGUI() {
            Buttons();

            PropertyFolds();

            BottomStuff();
        }

        protected void Buttons() {
            EditorGUILayout.BeginHorizontal();
            FoldButton("Gender", ref genderFold);
            FoldButton("Identity", ref identityFold);
            FoldButton("Stats", ref statsFold);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            FoldButton("Race", ref raceFold);
            FoldButton("Body", ref bodyFold);
            EditorGUILayout.EndHorizontal();
        }

        protected void FoldButton(string title, ref bool fold) {
            if (GUILayout.Button(title)) {
                CloseFolds();
                fold = true;
            }
        }

        protected void PropertyFolds() {
            if (genderFold) {
                EditorGUILayout.BeginVertical("box");
                serializedObject.Update();
                var essenceAmount = startGender.FindPropertyRelative("startEssence");
                var allowedGenders = startGender.FindPropertyRelative("allowedGenders");
                essenceAmount.intValue = EditorGUILayout.IntSlider("Essence", essenceAmount.intValue, 0, 9999);
                if (GUILayout.Button("Add gender option"))
                    allowedGenders.arraySize++;

                for (var i = 0; i < allowedGenders.arraySize; i++) {
                    EditorGUILayout.BeginHorizontal("box");
                    var arrayElementAtIndex = allowedGenders.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginVertical();
                    var odds = arrayElementAtIndex.FindPropertyRelative("weight");
                    EditorGUILayout.IntSlider(odds, 1, 5);
                    var gender = arrayElementAtIndex.FindPropertyRelative("gender");
                    EditorGUILayout.PropertyField(gender);
                    EditorGUILayout.EndVertical();
                    if (GUILayout.Button("Delete"))
                        allowedGenders.DeleteArrayElementAtIndex(i);
                    EditorGUILayout.EndHorizontal();
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            if (raceFold) {
                EditorGUILayout.BeginVertical("box");
                serializedObject.Update();
                EditorGUILayout.PropertyField(startRace);
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            BasicPropertyFold(identityFold, startIdentity);
            if (statsFold) {
                EditorGUILayout.BeginVertical("box");
                serializedObject.Update();
                var statSum = 0;
                var strength = startStats.FindPropertyRelative("strength");
                statSum += strength.intValue;
                var charm = startStats.FindPropertyRelative("charm");
                statSum += charm.intValue;
                var constitution = startStats.FindPropertyRelative("constitution");
                statSum += constitution.intValue;
                var intelligence = startStats.FindPropertyRelative("intelligence");
                statSum += intelligence.intValue;
                var agility = startStats.FindPropertyRelative("agility");
                statSum += agility.intValue;
                EditorGUILayout.LabelField($"Stat sum: {statSum}");
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            BasicPropertyFold(statsFold, startStats);
            if (bodyFold) {
                EditorGUILayout.BeginVertical("box");
                serializedObject.Update();
                var height = startBody.FindPropertyRelative("height");
                height.intValue = EditorGUILayout.IntSlider("Height", height.intValue, 1, 999);
                EditorGUILayout.HelpBox("Remember that race will modify height, 160 is average for race",
                    MessageType.Info);
                var muscle = startBody.FindPropertyRelative("muscle");
                muscle.intValue = EditorGUILayout.IntSlider("Muscle", muscle.intValue, 1, 99);
                var fat = startBody.FindPropertyRelative("fat");
                fat.intValue = EditorGUILayout.IntSlider("Fat", fat.intValue, 1, 99);
                var weight = muscle.intValue + fat.intValue + height.intValue / 2f * 0.40f;
                EditorGUILayout.LabelField($"Weight: {weight}");
                EditorGUILayout.LabelField($"Muscle {muscle.intValue / weight:0.##}% avg 28%");
                EditorGUILayout.LabelField($"Fat {fat.intValue / weight:0.##}% avg 25%");
                var rng = startBody.FindPropertyRelative("rng");
                EditorGUILayout.PropertyField(rng);
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }
        }

        void BasicPropertyFold(bool fold, SerializedProperty property) {
            if (fold) {
                EditorGUILayout.BeginVertical("box");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property);
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }
        }

        protected void BottomStuff() {
            baseEditorFold = EditorGUILayout.Foldout(baseEditorFold, "Base Editor");
            if (baseEditorFold)
                base.OnInspectorGUI();
            if (GUILayout.Button("Set all values to Default"))
                ((CharacterPreset)target).DefaultValues();
        }
    }
}