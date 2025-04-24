using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Threading.Tasks;

namespace UIToolkitDemo
{
    /// <summary>
    /// Manages task bar UI for opening SettingsView and ShopView 
    /// </summary>
    public class OptionsBarView : UIView
    {

        const float k_LerpTime = 0.6f; // seconds to interpolate

        VisualElement m_OptionsButton;
        VisualElement m_ShopGemButton;
        VisualElement m_ShopGoldButton;
        Label m_GoldLabel;
        Label m_GemLabel;

        public OptionsBarView(VisualElement topElement) : base(topElement)
        {
            ShopEvents.FundsUpdated += OnFundsUpdated;
        }

        public override void Dispose()
        {
            base.Dispose();
            ShopEvents.FundsUpdated -= OnFundsUpdated;

            UnregisterButtonCallbacks();
        }

        // identify visual elements by name
        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_OptionsButton = m_TopElement.Q("options-bar__button");
            m_ShopGoldButton = m_TopElement.Q("options-bar__gold-button");
            m_ShopGemButton = m_TopElement.Q("options-bar__gem-button");
            m_GoldLabel = m_TopElement.Q<Label>("options-bar__gold-count");
            m_GemLabel = m_TopElement.Q<Label>("options-bar__gem-count");
        }

        // set up button click events
        protected override void RegisterButtonCallbacks()
        {
            m_OptionsButton.RegisterCallback<ClickEvent>(ShowOptionsScreen);
            m_ShopGemButton.RegisterCallback<ClickEvent>(OpenGemShop);
            m_ShopGoldButton.RegisterCallback<ClickEvent>(OpenGoldShop);
        }

        // Optional: Unregistering the button callbacks is not strictly necessary
        // in most cases and depends on your application's lifecycle management.
        // You can choose to unregister them if needed for specific scenarios.
        protected void UnregisterButtonCallbacks()
        {
            m_OptionsButton.UnregisterCallback<ClickEvent>(ShowOptionsScreen);
            m_ShopGemButton.UnregisterCallback<ClickEvent>(OpenGemShop);
            m_ShopGoldButton.UnregisterCallback<ClickEvent>(OpenGoldShop);
        }

        // Handles the event when the SettingsView screen is shown.
        void ShowOptionsScreen(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            MainMenuUIEvents.SettingsScreenShown?.Invoke();

        }

        // Handles the event when the gold shop is opened.
        void OpenGoldShop(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            // Show the ShopScreen
            MainMenuUIEvents.OptionsBarShopScreenShown?.Invoke();

            // Open the tab to the gold products
            ShopEvents.TabSelected?.Invoke("gold");

        }

        // Handles the event when the gem shop is opened.
        void OpenGemShop(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            // Show the ShopScreen
            MainMenuUIEvents.OptionsBarShopScreenShown?.Invoke();

            // Open the tab to the gem product
            ShopEvents.TabSelected?.Invoke("gem");
        }

        void OnFundsUpdated(GameData gameData)
        {
            // Fire and forget
            _ = HandleFundsUpdatedAsync(gameData);
        }

        // Handles the funds update animation
        async Task HandleFundsUpdatedAsync(GameData gameData)
        {
            try
            {
                uint startGoldValue = (uint)Int32.Parse(m_GoldLabel.text);
                uint startGemsValue = (uint)Int32.Parse(m_GemLabel.text);

                await LerpRoutine(m_GoldLabel, startGoldValue, gameData.gold, k_LerpTime);
                await LerpRoutine(m_GemLabel, startGemsValue, gameData.gems, k_LerpTime);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[OptionsBarView] HandleFundsUpdatedAsync error: {ex.Message}");
            }
        }

        // Animated Label counter
        async Task LerpRoutine(Label label, uint startValue, uint endValue, float duration)
        {
            float lerpValue = (float)startValue;
            float t = 0f;
            label.text = string.Empty;

            while (Mathf.Abs((float)endValue - lerpValue) > 0.05f)
            {
                t += Time.deltaTime / k_LerpTime;

                lerpValue = Mathf.Lerp(startValue, endValue, t);
                label.text = lerpValue.ToString("0");
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            }
            label.text = endValue.ToString();
        }
    }

}

