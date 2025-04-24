using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace UIToolkitDemo
{
    // non-UI logic for HomeScreen
    public class HomeScreenController : MonoBehaviour
    {

        [Header("Level Data")]
        [SerializeField] LevelSO m_LevelData;

        [Header("Chat Data")]
        [SerializeField] string m_ChatResourcePath = "GameData/Chat";

        // List to store chat messages
        [SerializeField] List<ChatSO> m_ChatData = new List<ChatSO>();

        void Awake()
        {
            m_ChatData.AddRange(Resources.LoadAll<ChatSO>(m_ChatResourcePath));
        }

        void OnEnable()
        {
            HomeEvents.PlayButtonClicked += OnPlayGameLevel;
        }

        void OnDisable()
        {
            HomeEvents.PlayButtonClicked -= OnPlayGameLevel;
        }

        void Start()
        {
            HomeEvents.LevelInfoShown?.Invoke(m_LevelData);
            HomeEvents.ChatWindowShown?.Invoke(m_ChatData);
        }

        /// <summary>
        /// Play the game level.
        /// </summary>
        // scene-management methods
        public void OnPlayGameLevel()
        {
            // Do nothing without level data
            if (m_LevelData == null)
                return;

            // Notify about main menu exit
            HomeEvents.MainMenuExited?.Invoke();

            // Load the level scene if in the Unity Editor and playing
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SceneManager.LoadSceneAsync(m_LevelData.sceneName);
        }
    }
}
