using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Editor
{
    [CreateAssetMenu(menuName = "Create AnimatorControllerParameterGrabberSObj", fileName = "AnimatorControllerParameterGrabberSObj", order = 0)]
    [FilePath("EditorSavedSObj/GrabbedAnimatorParameters.foo", FilePathAttribute.Location.PreferencesFolder)]
    public class AnimatorControllerParameterGrabberSObj : ScriptableSingleton<AnimatorControllerParameterGrabberSObj>
    {
        [SerializeField] AnimatorController animatorController;
        [field: SerializeField] public List<NameAndHash> Parameters { get; private set; }
        void OnValidate()
        {
            Parameters = new List<NameAndHash>();
            if (animatorController == null) return;
            foreach (var para in animatorController.parameters)
                Parameters.Add(new NameAndHash(para.name, para.nameHash));
        }
        
        
        [Serializable]
        public struct NameAndHash
        {
            public NameAndHash(string name, int hash)
            {
                Name = name;
                Hash = hash;
            }

            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public int Hash { get; private set; }
        }
    }
}