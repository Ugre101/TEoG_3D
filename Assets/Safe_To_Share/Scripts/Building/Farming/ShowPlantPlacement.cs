using System;
using CustomClasses;
using Items;
using Safe_To_Share.Scripts.Farming.UI;
using Safe_To_Share.Scripts.Helpers;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.Farming
{
    public class ShowPlantPlacement : MonoBehaviour, ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] Camera cam;
        [SerializeField] LayerMask searchLayers;
        [SerializeField] float distFromPlayer = 12f;
        [SerializeField] [Range(0.1f, 0.4f)] float timeInterval = 0.2f;

        [SerializeField] [Range(float.Epsilon, 0.5f)]
        float tolerance = 0.2f;

        NativeArray<RaycastCommand> commands;
        JobHandle handle;
        bool hovering;

        PlantedPlant hoverPrefab;

        // InventoryItem item;
        Inventory inventory;

        string itemGuid;

        PlantedPlant lastPrefab;

        float lastTick;
        Vector2 lastValue;
        Vector2 newValue;
        Plant plant;
        PlantOptionButton plantButton;

        bool planting;
        NativeArray<RaycastHit> results;

        bool validPlacement;

        void Start()
        {
            results = new NativeArray<RaycastHit>(1, Allocator.Persistent);
            commands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            PlantOptionButton.ShowPlacement += ShowHowever;
            // optionButton.onClick.AddListener(DoInteraction);
        }

        void OnDisable()
        {
            if (hovering)
                CancelHovering();
        }

        void Update()
        {
            if (hovering is false) return;

            if (Time.time < lastTick + timeInterval)
                return;
            lastTick = Time.time;
            newValue = Pointer.current.position.ReadValue();
            if (Vector2.Distance(newValue, lastValue) < tolerance)
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

        public bool BlockIfActive()
        {
            if (!hovering) return false;
            CancelHovering();
            return true;
        }

        public static event Action UpdateSeedsOptions;

        void ShowHowever(Plant obj, Inventory inventory, string guid, PlantOptionButton button)
        {
            plant = obj;
            this.inventory = inventory;
            //    item = inventoryItem;
            itemGuid = guid;
            plantButton = button;
            StartHover(obj.Prefab);
        }

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
                var position = raycastHit.point;
                if (!(Vector3.Distance(PlayerPosition.Pos, position) < distFromPlayer)) continue;
                hoverPrefab.transform.position = position;
                validPlacement = true;
            }
        }

        public async void ConfirmPlacement(InputAction.CallbackContext callbackContext)
        {
            if (planting) return;
            if (callbackContext.performed is false) return;
            if (!validPlacement) return;
            planting = true;
            var planted = Instantiate(hoverPrefab);
            var position = hoverPrefab.transform.position;
            planted.Plant(new PlantStats(plant, position));
            var stillHave = await inventory.LowerItemAmountAndReturnIfStillHave(itemGuid);
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
            var camRay = cam.ScreenPointToRay(lastValue);
            commands[0] = new RaycastCommand(camRay.origin, camRay.direction, Mathf.Infinity, searchLayers);
            handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        }
    }
}