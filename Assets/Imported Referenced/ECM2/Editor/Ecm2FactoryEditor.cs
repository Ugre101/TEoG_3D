using Movement.ECM2.Source.Characters;
using Movement.ECM2.Source.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ECM2.Editor
{
    public static class Ecm2FactoryEditor
    {
        private static void InitCharacter(GameObject go)
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();

            rb.drag = 0.0f;
            rb.angularDrag = 0.0f;
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.freezeRotation = true;

            CapsuleCollider capsuleCollider = go.GetComponent<CapsuleCollider>();

            capsuleCollider.center = new Vector3(0f, 1f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 2.0f;
            capsuleCollider.material =
                AssetDatabase.LoadAssetAtPath<PhysicMaterial>(
                    "Assets/ECM2/Physic Materials/Frictionless.physicMaterial");

            PhysicMaterial physicMaterial = capsuleCollider.sharedMaterial;
            if (physicMaterial == null)
            {
                // if not founded, instantiate one and logs a warning message

                physicMaterial = new PhysicMaterial("Frictionless")
                {
                    dynamicFriction = 0.0f,
                    staticFriction = 0.0f,
                    bounciness = 0.0f,
                    frictionCombine = PhysicMaterialCombine.Multiply,
                    bounceCombine = PhysicMaterialCombine.Multiply
                };

                capsuleCollider.material = physicMaterial;

                Debug.LogWarning(
                    $"CharacterMovement: No 'PhysicMaterial' found for '{go.name}' CapsuleCollider, a frictionless one has been created and assigned.\n You should add a Frictionless 'PhysicMaterial' to game object '{go.name}'.");
            }
        }

        [MenuItem("GameObject/ECM2/Character", false, 0)]
        public static void CreateCharacter()
        {
            GameObject go = new GameObject("ECM2_Character", typeof(Rigidbody), typeof(CapsuleCollider), typeof(CharacterMovement), typeof(ECM2Character));

            InitCharacter(go);

            // Assign default input actions

            ECM2Character character = go.GetComponent<ECM2Character>();

            // Focus the newly created character

            Selection.activeGameObject = go;
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem("GameObject/ECM2/AgentCharacter", false, 0)]
        public static void CreateAgentCharacter()
        {
            GameObject go = new GameObject("ECM2_AgentCharacter", typeof(NavMeshAgent), typeof(Rigidbody),
                typeof(CapsuleCollider), typeof(CharacterMovement), typeof(AgentEcm2Character));

            InitCharacter(go);

            // Assign default input actions

            ECM2Character character = go.GetComponent<ECM2Character>();

            // Focus the newly created character

            Selection.activeGameObject = go;
            SceneView.FrameLastActiveSceneView();
        }
    }
}
