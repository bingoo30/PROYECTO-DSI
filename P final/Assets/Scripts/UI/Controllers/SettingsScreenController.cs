using System;
using UnityEngine;

namespace UIToolkitDemo
{
    // <summary>
    /// Manages the settings data and controls the flow of this data 
    /// between the SaveManager and the UI.
    /// </summary>
    public class SettingsScreenController : MonoBehaviour
    {
        GameData m_SettingsData;

        // Aspect ratio for Theme
        MediaAspectRatio m_MediaAspectRatio = MediaAspectRatio.Undefined;

        void OnEnable()
        {
            MediaQueryEvents.ResolutionUpdated += OnResolutionUpdated;
            SettingsEvents.UIGameDataUpdated += OnUISettingsUpdated;
            SaveManager.GameDataLoaded += OnGameDataLoaded;
        }

        void OnDisable()
        {
            MediaQueryEvents.ResolutionUpdated -= OnResolutionUpdated;
            SettingsEvents.UIGameDataUpdated -= OnUISettingsUpdated;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;
        }

        // Target frame rate based on radio button selection ( -1 = as fast as possible, 60fps, 30fps)
        void SelectTargetFrameRate(int selectedIndex)
        {
            // Convert button index to target frame rate
            switch (selectedIndex)
            {
               
                case 0:
                    SettingsEvents.TargetFrameRateSet?.Invoke(-1);
                    break;
                case 1:
                    SettingsEvents.TargetFrameRateSet?.Invoke(60);
                    break;
                case 2:
                    SettingsEvents.TargetFrameRateSet?.Invoke(30);
                    break;
                default:
                    SettingsEvents.TargetFrameRateSet?.Invoke(60);
                    break;
            }
        }

        // Receiving updated UI data 
        void OnUISettingsUpdated(GameData newSettingsData)
        {
            if (newSettingsData == null)
                return;

            m_SettingsData = newSettingsData;

            // Toggle the Fps Counter based on slide toggle position
            SettingsEvents.FpsCounterToggled?.Invoke(m_SettingsData.isSlideToggled);
            SelectTargetFrameRate(m_SettingsData.buttonSelection);

            // Notify the GameDataManager and other listeners
            SettingsEvents.SettingsUpdated?.Invoke(m_SettingsData);

            UpdateTheme();
        }

        // Sync loaded data from SaveManager to UI
        void OnGameDataLoaded(GameData gameData)
        {
            if (gameData == null)
                return;

            m_SettingsData = gameData;
            SettingsEvents.GameDataLoaded?.Invoke(m_SettingsData);
        }

        // Store portrait/landscape for Theme
        void OnResolutionUpdated(Vector2 resolution)
        {
            m_MediaAspectRatio = MediaQuery.CalculateAspectRatio(resolution);
        }

        void UpdateTheme()
        {
            // SettingsData stores the Theme as a string key. The basename is the aspect ratio (Landscape or Portrait) plus
            // a modifier for the seasonal decorations. This sample includes 6 themes (Landscape--Default, Landscape--Halloween,
            // Landscape--Christmas, Portrait--Default, Portrait--Halloween, Portrait--Christmas).

            string newTheme = m_MediaAspectRatio.ToString() + "--" + m_SettingsData.theme;
            ThemeEvents.ThemeChanged(newTheme);
        
        }
    }
}