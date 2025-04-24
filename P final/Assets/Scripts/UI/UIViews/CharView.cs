using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UIToolkitDemo
{
    public class CharView : UIView
    {
        const string k_LevelUpButtonInactiveClass = "footer__level-up-button--inactive";
        const string k_LevelUpButtonClass = "footer__level-up-button";

        Button[] m_GearSlots = new Button[4];

        Button m_LastCharButton;
        Button m_NextCharButton;
        Button m_AutoEquipButton;
        Button m_UnequipButton;
        Button m_LevelUpButton;

        Label m_CharacterLabel;
        Label m_PotionsForNextLevel;
        Label m_PotionCount;
        Label m_PowerLabel;

        VisualElement m_LevelUpButtonVFX;

        CharStatsView m_CharStatsView;  // Window that displays character stats

        Sprite m_EmptyGearSlotSprite;
        GameIconsSO m_GameIconsData;

        public CharView(VisualElement topElement) : base(topElement)
        {
            ShopEvents.PotionsUpdated += OnPotionUpdated;

            CharEvents.LevelUpButtonEnabled += OnLevelUpButtonEnabled;
            CharEvents.CharacterShown += OnCharacterUpdated;
            CharEvents.PreviewInitialized += OnInitialized;
            CharEvents.GearSlotUpdated += OnGearSlotUpdated;

            // Locate the empty gear slot sprite from a ScriptableObject icons
            m_GameIconsData = Resources.Load("GameData/GameIcons") as GameIconsSO;
            m_EmptyGearSlotSprite = m_GameIconsData.emptyGearSlotIcon;

            m_CharStatsView = new CharStatsView(topElement.Q<VisualElement>("CharStatsWindow"));
            m_CharStatsView.Show();
        }

        public override void Dispose()
        {
            base.Dispose();
            ShopEvents.PotionsUpdated -= OnPotionUpdated;

            CharEvents.LevelUpButtonEnabled -= OnLevelUpButtonEnabled;
            CharEvents.CharacterShown -= OnCharacterUpdated;
            CharEvents.PreviewInitialized -= OnInitialized;
            CharEvents.GearSlotUpdated -= OnGearSlotUpdated;

            UnregisterButtonCallbacks();
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_GearSlots[0] = m_TopElement.Q<Button>("char-inventory__slot1");
            m_GearSlots[1] = m_TopElement.Q<Button>("char-inventory__slot2");
            m_GearSlots[2] = m_TopElement.Q<Button>("char-inventory__slot3");
            m_GearSlots[3] = m_TopElement.Q<Button>("char-inventory__slot4");

            m_NextCharButton = m_TopElement.Q<Button>("char__next-button");
            m_LastCharButton = m_TopElement.Q<Button>("char__last-button");

            m_AutoEquipButton = m_TopElement.Q<Button>("char__auto-equip-button");
            m_UnequipButton = m_TopElement.Q<Button>("char__unequip-button");
            m_LevelUpButton = m_TopElement.Q<Button>("char__level-up-button");
            m_LevelUpButtonVFX = m_TopElement.Q<VisualElement>("char__level-up-button-vfx");

            m_CharacterLabel = m_TopElement.Q<Label>("char__label");
            m_PotionCount = m_TopElement.Q<Label>("char__potion-count");
            m_PotionsForNextLevel = m_TopElement.Q<Label>("char__potion-to-advance");
            m_PowerLabel = m_TopElement.Q<Label>("char__power-label");

        }

        // Optional: Unregistering the button callbacks is not strictly necessary
        // in most cases and depends on your application's lifecycle management.
        // You can choose to unregister them if needed for specific scenarios.
        protected override void RegisterButtonCallbacks()
        {
            m_GearSlots[0].RegisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[1].RegisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[2].RegisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[3].RegisterCallback<ClickEvent>(ShowInventory);

            m_NextCharButton.RegisterCallback<ClickEvent>(GoToNextCharacter);
            m_LastCharButton.RegisterCallback<ClickEvent>(GoToLastCharacter);

            m_AutoEquipButton.RegisterCallback<ClickEvent>(AutoEquipSlots);
            m_UnequipButton.RegisterCallback<ClickEvent>(UnequipSlots);
            m_LevelUpButton.RegisterCallback<ClickEvent>(LevelUpCharacter);
        }

        // Method to unregister all callbacks
        protected void UnregisterButtonCallbacks()
        {
            m_GearSlots[0].UnregisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[1].UnregisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[2].UnregisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[3].UnregisterCallback<ClickEvent>(ShowInventory);

            m_NextCharButton.UnregisterCallback<ClickEvent>(GoToNextCharacter);
            m_LastCharButton.UnregisterCallback<ClickEvent>(GoToLastCharacter);

            m_AutoEquipButton.UnregisterCallback<ClickEvent>(AutoEquipSlots);
            m_UnequipButton.UnregisterCallback<ClickEvent>(UnequipSlots);
            m_LevelUpButton.UnregisterCallback<ClickEvent>(LevelUpCharacter);
        }

        public override void Show()
        {
            base.Show();

            MainMenuUIEvents.TabbedUIReset?.Invoke("CharScreen");
            CharEvents.ScreenStarted?.Invoke();
        }

        public override void Hide()
        {
            base.Hide();
            CharEvents.ScreenEnded?.Invoke();
        }

        void LevelUpCharacter(ClickEvent evt)
        {
            CharEvents.LevelUpClicked?.Invoke();
        }

        // notify CharScreenController to unequip all gear
        public void UnequipSlots(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            CharEvents.GearAllUnequipped?.Invoke();
        }

        // notify CharScreenController to equip best gear available in empty slots
        public void AutoEquipSlots(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            CharEvents.GearAutoEquipped?.Invoke();
        }

        void GoToLastCharacter(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            CharEvents.LastCharacterSelected?.Invoke();
        }

        void GoToNextCharacter(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            CharEvents.NextCharacterSelected?.Invoke();
        }

        // open the inventory screen when clicking on a gear slot
        void ShowInventory(ClickEvent evt)
        {
            VisualElement clickedElement = evt.target as VisualElement;

            char slotNumber = clickedElement.name[clickedElement.name.Length - 1];
            int slot = (int)Char.GetNumericValue(slotNumber) - 1;

            AudioManager.PlayDefaultButtonSound();

            MainMenuUIEvents.InventoryScreenShown?.Invoke();

            CharEvents.InventoryOpened?.Invoke(slot);
        }

        public void UpdateCharacterStats(CharacterData characterToShow)
        {
            m_CharStatsView.UpdateWindow(characterToShow);
        }

        void UpdatePotionCountLabel()
        {
            if (m_PotionsForNextLevel == null)
                return;

            string potionsForNextLevelString = m_PotionsForNextLevel.text.TrimStart('/');

            if (potionsForNextLevelString != string.Empty)
            {
                int potionsForNextLevel = Int32.Parse(potionsForNextLevelString);
                int potionsCount = Int32.Parse(m_PotionCount.text);
                m_PotionCount.style.color = (potionsForNextLevel > potionsCount) ? new Color(0.88f, .36f, 0f) : new Color(0.81f, 0.94f, 0.48f);
            }
        }
        // event handling methods
        void OnInitialized()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }


        public void OnCharacterUpdated(CharacterData characterToShow)
        {
            if (characterToShow == null)
                return;

            m_CharacterLabel.text = characterToShow.CharacterBaseData.characterName;
            m_PowerLabel.text = characterToShow.GetCurrentPower().ToString();
            m_PotionsForNextLevel.text = "/" + characterToShow.GetXPForNextLevel().ToString();
            UpdatePotionCountLabel();
            UpdateCharacterStats(characterToShow);

            characterToShow.PreviewInstance.gameObject.SetActive(true);
        }

        void OnPotionUpdated(GameData gameData)
        {
            if (m_PotionCount == null)
                return;

            if (gameData == null)
                return;

            m_PotionCount.text = gameData.levelUpPotions.ToString();
            UpdatePotionCountLabel();
        }

        // Shows the correct sprite for each gear slot
        void OnGearSlotUpdated(EquipmentSO gearData, int slotToUpdate)
        {
            Button activeSlot = m_GearSlots[slotToUpdate];

            // plus symbol is the first child of char-inventory__slot-n
            VisualElement addSymbol = activeSlot.ElementAt(0);

            // background sprite is the second child of char-inventory__slot-n
            VisualElement gearElement = activeSlot.ElementAt(1);

            if (gearData == null)
            {
                if (gearElement != null)
                    gearElement.style.backgroundImage = new StyleBackground(m_EmptyGearSlotSprite);

                if (addSymbol != null)
                    addSymbol.style.display = DisplayStyle.Flex;
            }
            else
            {
                if (gearElement != null)
                    gearElement.style.backgroundImage = new StyleBackground(gearData.sprite);

                if (addSymbol != null)
                    addSymbol.style.display = DisplayStyle.None;
            }
        }

        // Toggle LevelUp VFX and button color based on available potions
        void OnLevelUpButtonEnabled(bool state)
        {
            if (m_LevelUpButtonVFX == null || m_LevelUpButton == null)
                return;

            m_LevelUpButtonVFX.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

            if (state)
            {
                // Enable the Button and allow the mouse pointer to activate the :hover pseudo-state
                m_LevelUpButton.SetEnabled(state);
                m_LevelUpButton.pickingMode = PickingMode.Position;

                // Add and remove the style classes to activate the Button
                m_LevelUpButton.AddToClassList(k_LevelUpButtonClass);
                m_LevelUpButton.RemoveFromClassList(k_LevelUpButtonInactiveClass);

            }
            else
            {
                // Disable the Button and don't allow the mouse pointer to activate the :hover pseudo-state
                m_LevelUpButton.SetEnabled(state);
                m_LevelUpButton.pickingMode = PickingMode.Ignore;
                m_LevelUpButton.AddToClassList(k_LevelUpButtonInactiveClass);
                m_LevelUpButton.RemoveFromClassList(k_LevelUpButtonClass);

            }
        }
    }
}