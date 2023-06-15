﻿using Character.Family;
using Character.IdentityStuff;
using Character.PlayerStuff;
using Character.PregnancyStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEditor;
using UnityEngine;

namespace EditorFolder {
    public sealed class DayCareWindow : EditorWindow {
        void OnGUI() {
            GUILayout.Label("Day care children", EditorStyles.boldLabel);
            if (GUILayout.Button("Gain Child")) {
                var holder = FindObjectOfType<PlayerHolder>();
                Fetus fetus = new(new Player(), holder.Player);
                var child = fetus.GetBorn("Testis");
                holder.Player.FamilyTree.Children.Add(child.Identity.ID);
                DayCare.AddChild(child);
                DayCare.AddChild(fetus.GetBorn("Do I Count"));
            }

            foreach (var child in DayCare.ChildDict.Values) {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.TextField("Name", child.Identity.FullName);
                EditorGUILayout.IntField("Age", child.Identity.BirthDay.DaysOld());
                EditorGUILayout.TextField("Mother", child.FamilyTree.Mother.FullName);
                EditorGUILayout.TextField("Father", child.FamilyTree.Father.FullName);
                EditorGUILayout.EndVertical();
            }
        }

        [MenuItem("MENUITEM/Day care")]
        static void ShowWindow() {
            var window = GetWindow<DayCareWindow>();
            window.titleContent = new GUIContent("Day care");
            window.Show();
        }
    }
}