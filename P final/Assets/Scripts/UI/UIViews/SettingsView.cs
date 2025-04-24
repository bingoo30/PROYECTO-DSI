using UnityEngine;
using UnityEngine.UIElements;
using MyUILibrary;

namespace UIToolkitDemo
{

    // This controls general settings for the game. Many of these options are non-functional in this demo but
    // show how to send data to the SettingsController for syncing with the GameDataManager.
    public class SettingsView : UIView
    {
        // These class selectors hide/show the settings screen overlay.
        const string k_ScreenActiveClass = "settings__screen";
        const string k_ScreenInactiveClass = "settings__screen--inactive";

        // Visual elements
        Button m_ResetLevelButton;
        Button m_ResetFundsButton;
        TextField m_PlayerTextfield;
        Toggle m_ExampleToggle;
        DropdownField m_ThemeDropdown;
        DropdownField m_ExampleDropdown;
        Slider m_MusicSlider;
        Slider m_SfxSlider;
        SlideToggle m_SlideToggle;

        RadioButtonGroup m_RadioButtonGroup;
        Button m_BackButton;

        // Top UI element for transitions
        VisualElement m_ScreenContainer;

        // Temporary storage to send settings data back to SettingsController
        GameData m_LocalUISettings = new GameData();

        public SettingsView (VisualElement topElement): base(topElement)
        {
            // Sets m_SettingsData using previously saved data
            SettingsEvents.GameDataLoaded += OnGameDataLoaded;

            // Hide/disable by default
            m_ScreenContainer.AddToClassList(k_ScreenInactiveClass);
            m_ScreenContainer.RemoveFromClassList(k_ScreenActiveClass);
        }

        public override void Dispose()
        {
            base.Dispose();
            SettingsEvents.GameDataLoaded -= OnGameDataLoaded;
        }

        public override void Show()
        {
            base.Show();

            // Use styles to fade in with transition
            m_ScreenContainer.RemoveFromClassList(k_ScreenInactiveClass);
            m_ScreenContainer.AddToClassList(k_ScreenActiveClass);

            // Notify GameDataManager
            SettingsEvents.SettingsShown?.Invoke();
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_BackButton = m_TopElement.Q<Button>("settings__panel-back-button");
            m_ResetLevelButton = m_TopElement.Q<Button>("settings__social-button1");
            m_ResetFundsButton = m_TopElement.Q<Button>("settings__social-button2");
            m_PlayerTextfield = m_TopElement.Q<TextField>("settings__player-textfield");
            m_ExampleToggle = m_TopElement.Q<Toggle>("settings__toggle");
            m_ThemeDropdown = m_TopElement.Q<DropdownField>("settings__theme-dropdown");
            m_ExampleDropdown = m_TopElement.Q<DropdownField>("settings__dropdown");
            m_MusicSlider = m_TopElement.Q<Slider>("settings__slider1");
            m_SfxSlider = m_TopElement.Q<Slider>("settings__slider2");
            m_SlideToggle = m_TopElement.Q<SlideToggle>("settings__slide-toggle");
            m_RadioButtonGroup = m_TopElement.Q<RadioButtonGroup>("settings__radio-button-group");

            m_ScreenContainer = m_TopElement.Q<VisualElement>("settings__screen");
        }

        // Note: unregistering the button callbacks is optional and omitted in this case. Use the
        // UnregisterCallback and UnregisterValueChangedCallback methods to unregister callbacks
        // when necessary.
        protected override void RegisterButtonCallbacks()
        {
            m_BackButton.RegisterCallback<ClickEvent>(CloseWindow);

            m_ResetLevelButton.RegisterCallback<ClickEvent>(ResetLevel);
            m_ResetFundsButton.RegisterCallback<ClickEvent>(ResetFunds);

            m_PlayerTextfield.RegisterCallback<KeyDownEvent>(SetPlayerTextfield);
            m_ThemeDropdown.RegisterValueChangedCallback(ChangeThemeDropdown);
            m_ThemeDropdown.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_ExampleDropdown.RegisterValueChangedCallback(ChangeDropdown);
            m_ExampleDropdown.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_MusicSlider.RegisterValueChangedCallback(ChangeMusicVolume);
            m_MusicSlider.RegisterCallback<PointerCaptureOutEvent>(evt => SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings));
            m_MusicSlider.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());


            m_SfxSlider.RegisterValueChangedCallback(ChangeSfxVolume);
            m_SfxSlider.RegisterCallback<PointerCaptureOutEvent>(evt => SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings));
            m_SfxSlider.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_ExampleToggle.RegisterValueChangedCallback(ChangeToggle);
            m_ExampleToggle.RegisterCallback<ClickEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_SlideToggle.RegisterValueChangedCallback(ChangeSlideToggle);
            m_SlideToggle.RegisterCallback<ClickEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_RadioButtonGroup.RegisterCallback<ChangeEvent<int>>(ChangeRadioButton);
        }


        // Callback function for SlideToggle change event
        void ChangeSlideToggle(ChangeEvent<bool> evt)
        {

            // non-functional setting for demo purposes
            m_LocalUISettings.isSlideToggled = evt.newValue;

            SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);
        }

        // Callback function for Toggle change event
        void ChangeToggle(ChangeEvent<bool> evt)
        {
            // non-functional setting for demo purposes
            m_LocalUISettings.isToggled = evt.newValue;

            // notify the GameDataManager
            SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);
        }

        // Callback function for Sfx volume Slider change event
        void ChangeSfxVolume(ChangeEvent<float> evt)
        {
            evt.StopPropagation();
            m_LocalUISettings.sfxVolume = evt.newValue;
        }

        // Callback function for Music volume Slider change event
        void ChangeMusicVolume(ChangeEvent<float> evt)
        {
            evt.StopPropagation();
            m_LocalUISettings.musicVolume = evt.newValue;

        }

        // Callback function for Example dropdown change event
        void ChangeDropdown(ChangeEvent<string> evt)
        {
            // non-functional setting for demo purposes
            m_LocalUISettings.dropdownSelection = evt.newValue;

        }

        // Callback function for RadioButton change event
        void ChangeRadioButton(ChangeEvent<int> evt)
        {
            AudioManager.PlayDefaultButtonSound();

            // non-functional setting for demo purposes
            m_LocalUISettings.buttonSelection = evt.newValue;

            // notify the GameDataManager
            SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);
        }

        // Callback function for ResetLevel button change event
        void ResetLevel(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            SettingsEvents.PlayerLevelReset?.Invoke();
        }

        // Callback function for ResetFunds button change event
        void ResetFunds(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            SettingsEvents.PlayerFundsReset?.Invoke();
        }

        // Change ThemeStyleSheets on dropdown
        void ChangeThemeDropdown(ChangeEvent<string> evt)
        {
            // Save the theme name string in the Game Data
            m_LocalUISettings.theme = evt.newValue;

            SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);
        }

        // Save the player name when hitting Return/Enter
        void SetPlayerTextfield(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return && m_LocalUISettings != null)
            {
                m_LocalUISettings.username = m_PlayerTextfield.text;
                SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);
            }
        }

        // Close this UI window
        void CloseWindow(ClickEvent evt)
        {
            m_ScreenContainer.RemoveFromClassList(k_ScreenActiveClass);
            m_ScreenContainer.AddToClassList(k_ScreenInactiveClass);

            AudioManager.PlayDefaultButtonSound();

            SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);

            Hide();
        }


        // Event-handling methods

        // Syncs saved data from the GameDataManager to the UI elements
        void OnGameDataLoaded(GameData loadedGameData)
        {
            if (loadedGameData == null)
                return;

            m_LocalUISettings = loadedGameData;

            // Update the UI with saved values
            m_PlayerTextfield.value = loadedGameData.username;
            m_ExampleDropdown.value = loadedGameData.dropdownSelection;
            m_ThemeDropdown.value = loadedGameData.theme;

            m_RadioButtonGroup.value = loadedGameData.buttonSelection;

            m_MusicSlider.value = loadedGameData.musicVolume;
            m_SfxSlider.value = loadedGameData.sfxVolume;

            m_SlideToggle.value = loadedGameData.isSlideToggled;


            m_ExampleToggle.value = loadedGameData.isToggled;

            SettingsEvents.UIGameDataUpdated?.Invoke(m_LocalUISettings);
        }
    }
}