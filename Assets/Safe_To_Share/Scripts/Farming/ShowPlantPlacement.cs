using System;
using AvatarStuff.Holders;
using Items;
using Safe_To_Share.Scripts.Farming.UI;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.Farming
{
    public class ShowPlantPlacement : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] LayerMask searchLayers;
        [SerializeField] InputActionReference hotKey;
        [SerializeField] float cameraRayDist = 12f;
        const int FrameLimit = 10;
         NativeArray<RaycastCommand> commands;
        JobHandle handle;
        NativeArray<RaycastHit> results;
        PlantedPlant hoverPrefab;
        bool hovering;
        Plant plant;
        InventoryItem item;

        bool validPlacement;
        void Start()
        {
            results = new NativeArray<RaycastHit>(1, Allocator.Persistent); 
            commands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            PlantOptionButton.ShowPlacement += ShowHowever;
            // optionButton.onClick.AddListener(DoInteraction);
        }

        void OnDestroy()
        {
            PlantOptionButton.ShowPlacement -= ShowHowever;
            results.Dispose();
            commands.Dispose();
        }

        void ShowHowever( Plant obj, InventoryItem inventoryItem)
        {
            plant = obj;
            item = inventoryItem;
            StartHover(obj.Prefab);
        }

        public void StartHover(PlantedPlant prefab)
        {
            hoverPrefab = Instantiate(prefab); // just dump it
            hovering = true;
            validPlacement = false;
        }

        public void CancelHovering()
        {
            Destroy(hoverPrefab);
            hovering = false;
            validPlacement = false;
        }
        void Update()
        {
            if (hovering is false) return;
                
            if (Time.frameCount % FrameLimit != 0)
                return;
            ScheduleRayCast();
            CheckResults();
        }

        void CheckResults()
        {
            handle.Complete();
            foreach (RaycastHit raycastHit in results)
            {
                if (raycastHit.collider is TerrainCollider)
                {
                    hoverPrefab.transform.position = raycastHit.point;
                    validPlacement = true;
                }
            }
        }

        public void ConfirmPlacement()
        {
            if (validPlacement)
            {
                hoverPrefab.Plant(new PlantStats(plant,hoverPrefab.transform.position));
                item.Amount--;
                if (item.Amount <= 0)
                {
                    // Stop
                }
            }
        }
        void ScheduleRayCast()
        {
            Ray camRay = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
            commands[0] = new RaycastCommand(camRay.origin, camRay.direction, cameraRayDist, searchLayers);
            handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        }
    }
}