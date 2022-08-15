using System;
using CustomClasses;
using Items;
using Safe_To_Share.Scripts.Farming.UI;
using Safe_To_Share.Scripts.Helpers;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Safe_To_Share.Scripts.Farming
{
    public class ShowPlantPlacement : MonoBehaviour,ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] Camera cam;
        [SerializeField] LayerMask searchLayers;
        [SerializeField] float distFromPlayer = 12f;
        [SerializeField, Range(0.1f, 1f),] float timeInterval = 0.5f;
        [SerializeField] PlantFarmAreaPlants areaPlants;
        NativeArray<RaycastCommand> commands;
        JobHandle handle;
        bool hovering;
        PlantedPlant hoverPrefab;
        InventoryItem item;
        Inventory inventory;

        float lastTick = 0;
        Plant plant;
        NativeArray<RaycastHit> results;

        bool validPlacement;
        Vector2 lastValue;
        Vector2 newValue;
        [SerializeField,Range(float.Epsilon,1f)] float tolerance = 0.2f;

        public static event Action UpdateSeedsOptions;
        void Start()
        {
            results = new NativeArray<RaycastHit>(1, Allocator.Persistent);
            commands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            PlantOptionButton.ShowPlacement += ShowHowever;
            // optionButton.onClick.AddListener(DoInteraction);
        }
        void OnEnable()
        {
        }

        void OnDisable()
        {
        }


        void Update()
        {
            if (hovering is false) return;

            if (Time.time < lastTick + timeInterval)
                return;
            lastTick = Time.time;
            newValue = Pointer.current.position.ReadValue();
            if (Vector2.Distance(newValue,lastValue) < tolerance)
                return;
            ScheduleRayCast();
            CheckResults();
            lastValue = newValue;
        }

        void OnDestroy()
        {
            PlantOptionButton.ShowPlacement -= ShowHowever;
            results.Dispose();
            commands.Dispose();
        }

        PlantOptionButton plantButton;
        void ShowHowever(Plant obj,Inventory inventory, InventoryItem inventoryItem, PlantOptionButton button)
        {
            plant = obj;
            this.inventory = inventory;
            item = inventoryItem;
            plantButton = button;
            StartHover(obj.Prefab);
        }

        PlantedPlant lastPrefab;

        public void StartHover(PlantedPlant prefab)
        {
            lastPrefab = prefab;
            hoverPrefab = Instantiate(prefab); // just dump it
            hovering = true;
            validPlacement = false;
        }

        public void CancelHovering()
        {
            Destroy(hoverPrefab.gameObject);
            hoverPrefab = null;
            hovering = false;
            validPlacement = false;
        }

        void CheckResults()
        {
            handle.Complete();
            foreach (var raycastHit in results)
            {
                if (raycastHit.collider is not TerrainCollider) continue;
                Vector3 position = raycastHit.point;
                if (!(Vector3.Distance(PlayerPosition.Pos, position) < distFromPlayer)) continue;
                hoverPrefab.transform.position = position;
                validPlacement = true;
            }
        }

        bool planting;
        public async void ConfirmPlacement(InputAction.CallbackContext callbackContext)
        {
            if (planting) return;
            if (callbackContext.performed is false) return;
            if (!validPlacement) return;
            planting = true;
            var planted = Instantiate(hoverPrefab);
            var position = hoverPrefab.transform.position;
            areaPlants.AddToArea(planted,new PlantStats(plant,position));
            planted.Plant(new PlantStats(plant, position));
            bool stillHave = await inventory.LowerItemAmountAndReturnIfStillHave(item);
            if (stillHave is false)
            {
                // Stop
                CancelHovering();
                Destroy(plantButton.gameObject);
                UpdateSeedsOptions?.Invoke();
            }
            else
            {
                hovering = true;
            }
            validPlacement = false;
            planting = false;
        }
        
    
        
        void ScheduleRayCast()
        {
            Ray camRay = cam.ScreenPointToRay(lastValue);
            commands[0] = new RaycastCommand(camRay.origin, camRay.direction, Mathf.Infinity, searchLayers);
            handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        }

        public bool BlockIfActive()
        {
            if (!hovering) return false;
            CancelHovering();
            return true;

        }
    }
}