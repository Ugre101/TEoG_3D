using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveStuff {
    [Serializable]
    public struct SaveSummary {
        [SerializeField] string playerName;
        [SerializeField] int level;
        [SerializeField] string sceneName;
        [SerializeField] string date;
        [SerializeField] string playerAddedText;

        public SaveSummary(string playerName, int level, string addedText) {
            this.playerName = playerName;
            this.level = level;
            playerAddedText = addedText;
            sceneName = SceneManager.GetActiveScene().name;
            date = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }

        public string PlayerName => playerName;

        public int Level => level;

        public string SceneName => sceneName;

        public string Date => date;

        public string PlayerAddedText => playerAddedText;
    }
}