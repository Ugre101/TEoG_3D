using System;
using System.Collections;
using Character.Ailments;
using Character.PlayerStuff;
using Character.RelationShipStuff;
using Dialogue;
using GameUIAndMenus;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class TestStuff : MonoBehaviour
    {
        Player testPlayer = new Player();
        enum textNum
        {
            test1 = 0,
            test2 = 4,
            test3 = 8,
        }
        [SerializeField] RelationsShips relationsShips = new RelationsShips();
        [SerializeField] BaseDialogue testDial;
        [SerializeField] GameCanvas gameCanvas;


        Tired tired = new Tired();
        DeadTired deadTired = new DeadTired();
        [ContextMenu("Test")]
        void Test()
        {
            StartCoroutine(AddEvents());
        }

        [SerializeField, Range(-100, 500),] int sleepQuality = 0;
        [ContextMenu("Sleep")]
        void Sleep()
        {
            //  PlayerHolder.Instance.Sleep(sleepQuality);
        }

        IEnumerator AddEvents()
        {
            for (int i = 0; i < 12; i++)
            {
                EventLog.AddEvent($"Event {i}");
                yield return new WaitForSeconds(2.5f);
            }
        }

        [ContextMenu("Test Dialogue")]
        void TestDialogue() => gameCanvas.OpenDialogueMenu(testDial);
        [Serializable]
        struct SaveNewLine
        {
            public string Wow;
        }
    }
}