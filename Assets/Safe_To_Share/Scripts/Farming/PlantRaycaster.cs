using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.Farming
{
    public class PlantRaycaster : MonoBehaviour
    {

        [SerializeField, Range(1f, 3f)] float placeRange = 1f;
        [SerializeField] LayerMask canBuildOn;
        NativeArray<RaycastCommand> commands;
        JobHandle handle;
        NativeArray<RaycastHit> results;
        Camera main;

        
        void Start()
        {
            results = new NativeArray<RaycastHit>(1, Allocator.Persistent);
            commands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            main = Camera.main;
        }

        void Update()
        {
            throw new NotImplementedException();
        }

        public void FirstPerson()
        {
            Ray ray = main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            commands[0] = new RaycastCommand(ray.origin, ray.direction, placeRange, canBuildOn);

            handle = RaycastCommand.ScheduleBatch(commands, results, 1);
            handle.Complete();
            RaycastHit batchedHit = results[0];
            if (batchedHit.collider != null)
            {
                
            }
            // Center ray
        }

        public void ThirdPerson()
        {
            if (Pointer.current == null) return;
            Ray ray = main.ScreenPointToRay(Pointer.current.position.ReadValue());
            commands[0] = new RaycastCommand(ray.origin, ray.direction, placeRange, canBuildOn);

            handle = RaycastCommand.ScheduleBatch(commands, results, 1);
            handle.Complete();

            RaycastHit batchedHit = results[0];
            if (batchedHit.collider != null)
            {
                
            }
            // Mouse pos
        }
    }
}