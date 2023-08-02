using System;
using System.Collections;
using Character.Ailments;
using Character.PlayerStuff;
using Character.RelationShipStuff;
using Dialogue;
using Safe_To_Share.Scripts.GameUIAndMenus;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts {
    public sealed class TestStuff : MonoBehaviour {
        [SerializeField] RelationsShips relationsShips = new();
        [SerializeField] BaseDialogue testDial;
        [SerializeField] GameCanvas gameCanvas;

        [SerializeField, Range(-100, 500),] int sleepQuality;
        DeadTired deadTired = new();
        Player testPlayer = new();


        Tired tired = new();

        [ContextMenu("Test")]
        void Test() {
            print(nameof(TestStuff));
            //StartCoroutine(AddEvents());
        }

        [ContextMenu("Sleep")]
        void Sleep() {
            //  PlayerHolder.Instance.Sleep(sleepQuality);
        }

        IEnumerator AddEvents() {
            for (var i = 0; i < 12; i++) {
                EventLog.AddEvent($"Event {i}");
                yield return new WaitForSeconds(2.5f);
            }
        }

        [ContextMenu("Test Dialogue")]
        void TestDialogue() => gameCanvas.OpenDialogueMenu(testDial);

        enum textNum {
            test1 = 0, test2 = 4, test3 = 8,
        }

        [Serializable]
        struct SaveNewLine {
            public string Wow;
        }
    }
}