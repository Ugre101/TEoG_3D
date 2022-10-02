using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Static
{
    public static class GameManager
    {
        public enum EnemyClose
        {
            OutOfRange,
            InView,
            Chasing,
        }
        
        public enum GameState
        {
            FreePlay,
            Paused,
        }

        public static Action<GameState> StateChange;
        static GameState currentState;

        public static GameState CurrentState => currentState;

        static void SetCurrentState(GameState value)
        {
            currentState = value;
            StateChange?.Invoke(value);
        }

        static bool cursorOrgState;
        static CursorLockMode lockState;

        public static Action<EnemyClose> EnemyGrowsCloser;
        public static bool Paused { get; private set; }

        public static void Pause()
        {
            if (Paused)
                return;
            Paused = true;
            Time.timeScale = 0f;
            cursorOrgState = Cursor.visible;
            Cursor.visible = true;
            lockState = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
            SetCurrentState(GameState.Paused);
        }


        public static void Resume(bool forceFreeCursor)
        {
            Paused = false;
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
            SetCurrentState(GameState.FreePlay);
        }

        public static void EnemyInRange(EnemyClose howClose)
        {
        }
    }
}