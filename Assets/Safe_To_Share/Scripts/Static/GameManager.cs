using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Static
{
    public static class GameManager
    {
        private static bool cursorOrgState;
        private static CursorLockMode lockState;
        static bool paused;
        public static void Pause()
        {
            if (paused)
                return;
            paused = true;
            Time.timeScale = 0f;
            cursorOrgState = Cursor.visible;
            Cursor.visible = true;
            lockState = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void Resume(bool forceFreeCursor)
        {
            paused = false;
            Time.timeScale = 1f;
            if (forceFreeCursor)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                
                Cursor.visible = cursorOrgState;
                Cursor.lockState = lockState;
            }
        }

        public static void EnemyInRange(EnemyClose howClose)
        {
            
        }

        public static Action<EnemyClose> EnemyGrowsCloser;
        public enum EnemyClose
        {
            OutOfRange,
            InView,
            Chasing,
        }

    }
}
