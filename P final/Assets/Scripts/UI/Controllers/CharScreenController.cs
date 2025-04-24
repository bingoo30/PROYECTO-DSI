using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UIToolkitDemo
{
    // non-UI logic for the CharScreen
    public class CharScreenController : MonoBehaviour
    {

        [Tooltip("Characters to choose from.")]
        [SerializeField] List<CharacterData> m_Characters;

        [Tooltip("Parent transform for all character previews.")]
        [SerializeField] Transform m_previewTransform;

        [Header("Inventory")]
        [Tooltip("Check this option to allow only one type of gear (armor, weapon, etc.) per character.")]
        [SerializeField] bool m_UnequipDuplicateGearType;

        [Header("Level Up")]
        [SerializeField] [Tooltip("Controls playback of level up FX.")] PlayableDirector m_LevelUpPlayable;

        public CharacterData CurrentCharacter { get => m_Characters[m_CurrentIndex]; }

        int m_CurrentIndex;
        int m_ActiveGearSlot;


        void OnEnable()
        {
            CharEvents.LevelIncremented += OnLevelIncremented;

            CharEvents.ScreenStarted += OnCharScreenStarted;
            CharEvents.ScreenEnded += OnCharScreenEnded;
            CharEvents.NextCharacterSelected += SelectNextCharacter;
            CharEvents.LastCharacterSelected += SelectLastCharacter;
            CharEvents.InventoryOpened += OnInventoryOpened;
            CharEvents.GearAutoEquipped += OnGearAutoEquipped;
            CharEvents.GearAllUnequipped += OnGearUnequipped;
            CharEvents.LevelUpClicked += OnLevelUpClicked;
            CharEvents.CharacterLeveledUp += OnCharacterLeveledUp;

            InventoryEvents.GearSelected += OnGearSelected;
            InventoryEvents.GearAutoEquipped += OnGearAutoEquipped;

            SettingsEvents.PlayerLevelReset += OnResetPlayerLevel;
        }

        void OnDisable()
        {
            CharEvents.LevelIncremented -= OnLevelIncremented;

            CharEvents.ScreenStarted -= OnCharScreenStarted;
            CharEvents.ScreenEnded -= OnCharScreenEnded;

            CharEvents.NextCharacterSelected -= SelectNextCharacter;
            CharEvents.LastCharacterSelected -= SelectLastCharacter;
            CharEvents.InventoryOpened -= OnInventoryOpened;
            CharEvents.GearAutoEquipped -= OnGearAutoEquipped;
            CharEvents.GearAllUnequipped -= OnGearUnequipped;
            CharEvents.LevelUpClicked -= OnLevelUpClicked;
            CharEvents.CharacterLeveledUp -= OnCharacterLeveledUp;

            InventoryEvents.GearSelected -= OnGearSelected;
            InventoryEvents.GearAutoEquipped -= OnGearAutoEquipped;

            SettingsEvents.PlayerLevelReset -= OnResetPlayerLevel;
        }

        void Awake()
        {
            InitializeCharPreview();
        }

        void Start()
        {
            InitGearData();
            UpdateLevelMeter();
        }

        void UpdateView()
        {
            if (m_Characters.Count == 0)
                return;

            // show the Character Prefab
            CharEvents.CharacterShown?.Invoke(CurrentCharacter);

            // update the four gear slots
            UpdateGearSlots();

            UpdateLevelMeter();
        }

        // character preview methods
        public void SelectNextCharacter()
        {
            if (m_Characters.Count == 0)
                return;

            ShowCharacterPreview(false);

            m_CurrentIndex++;
            if (m_CurrentIndex >= m_Characters.Count)
                m_CurrentIndex = 0;

            // select next character from m_Characters and refresh the CharScreen
            UpdateView();
        }

        public void SelectLastCharacter()
        {
            if (m_Characters.Count == 0)
                return;

            ShowCharacterPreview(false);

            m_CurrentIndex--;
            if (m_CurrentIndex < 0)
                m_CurrentIndex = m_Characters.Count - 1;

            // select last character from m_Characters and refresh the CharScreen
            UpdateView();
        }

        // preview GameObject for each character
        void InitializeCharPreview()
        {
            foreach (CharacterData charData in m_Characters)
            {
                if (charData == null)
                {
                    Debug.LogWarning("CharScreenController.InitializeCharPreview Warning: Missing character data.");
                    continue;
                }
                GameObject previewInstance = Instantiate(charData.CharacterBaseData.characterVisualsPrefab);

                previewInstance.transform.SetParent(m_previewTransform);
                previewInstance.transform.localPosition = Vector3.zero;
                previewInstance.transform.localRotation = Quaternion.identity;
                previewInstance.transform.localScale = Vector3.one;
                charData.PreviewInstance = previewInstance;
                previewInstance.gameObject.SetActive(false);
            }

            CharEvents.PreviewInitialized?.Invoke();
       
        }

        void ShowCharacterPreview(bool state)
        {
            if (m_Characters.Count == 0)
                return;

            CharacterData currentCharacter = m_Characters[m_CurrentIndex];
            currentCharacter.PreviewInstance.gameObject.SetActive(state);
            UpdateLevelMeter();
        }

        // Assign starting gear for each character based on CharacterBaseSO
        void InitGearData()
        {
            // notify InventoryScreenController
            CharEvents.GearDataInitialized?.Invoke(m_Characters);
        }

        void UpdateGearSlots()
        {
            if (CurrentCharacter == null)
                return;

            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                CharEvents.GearSlotUpdated?.Invoke(CurrentCharacter.GearData[i], i);
            }
        }

        // Removes a specific EquipmentType (helmet, shield/armor, weapon, gloves, boots) from a character;
        // use this to prevent duplicate gear types from appearing in the inventory.
        public void RemoveGearType(EquipmentType typeToRemove)
        {
            if (CurrentCharacter == null)
                return;

            // remove type from each character's inventory slot if found; notifies CharScreen
            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                if (CurrentCharacter.GearData[i] != null && CurrentCharacter.GearData[i].equipmentType == typeToRemove)
                {
                    CharEvents.GearItemUnequipped(CurrentCharacter.GearData[i]);
                    CurrentCharacter.GearData[i] = null;
                    CharEvents.GearSlotUpdated?.Invoke(null, i);
                }
            }
        }

        // Character level methods
        int GetCharLevels()
        {
            int totalLevels = 0;
            foreach (CharacterData charData in m_Characters)
            {
                totalLevels += charData.CurrentLevel;
            }
            return totalLevels;
        }

        // Update the upper left level meter
        void UpdateLevelMeter()
        {
            int totalLevels = GetCharLevels();
            CharEvents.LevelUpdated?.Invoke((float)totalLevels);
        }

        // Event-handling methods

        private void OnResetPlayerLevel()
        {
            foreach (CharacterData charData in m_Characters)
            {
                charData.CurrentLevel = 0;
            }
            CharEvents.CharacterShown?.Invoke(CurrentCharacter);
            UpdateLevelMeter();
        }
        void OnCharScreenStarted()
        {
            UpdateView();
            ShowCharacterPreview(true);
        }

        void OnCharScreenEnded()
        {
            ShowCharacterPreview(false);
        }

        // click the level up button
        void OnLevelUpClicked()
        {
            // notify GameDataManager that we want to spend LevelUpPotion
            CharEvents.LevelPotionUsed?.Invoke(CurrentCharacter);
        }

        // update the character stats UI
        void OnLevelIncremented(CharacterData charData)
        {
            if (charData == CurrentCharacter)
            {
                CharEvents.CharacterShown?.Invoke(CurrentCharacter);
                UpdateLevelMeter();
            }
        }

        // success or failure when leveling up a character 
        void OnCharacterLeveledUp(bool didLevel)
        {
            if (didLevel)
            {
                //increment the Player Level
                CurrentCharacter.IncrementLevel();

                // playback the FX sequence
                m_LevelUpPlayable.Play();
            }
        }

        // track the gear slot used to open the Inventory
        void OnInventoryOpened(int gearSlot)
        {
            m_ActiveGearSlot = gearSlot;
        }

        // Handles gear selection from the Inventory Screen
        void OnGearSelected(EquipmentSO gearObject)
        {
            // If Slot already has an item, notify the InventoryScreenController and return it to the inventory
            if (CurrentCharacter.GearData[m_ActiveGearSlot] != null)
            {

                CharEvents.GearItemUnequipped?.Invoke(CurrentCharacter.GearData[m_ActiveGearSlot]);
                CurrentCharacter.GearData[m_ActiveGearSlot] = null;
            }

            // Remove any duplicate EquipmentTypes (only permit one type of helmet, shield/armor, weapon, gloves, or boots)
            if (m_UnequipDuplicateGearType)
            {
                RemoveGearType(gearObject.equipmentType);
            }

            // Set the Gear into the active slot
            CurrentCharacter.GearData[m_ActiveGearSlot] = gearObject;

            // Notify CharScreen to update
            CharEvents.GearSlotUpdated?.Invoke(gearObject, m_ActiveGearSlot);
        }

        // Unequip all gear slots
        void OnGearUnequipped()
        {
            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                // If we currently have Equipment in one of the four gear slots, remove it
                if (CurrentCharacter.GearData[i] != null)
                {
                    // Notifies the InventoryScreenController to unequip gear and update lists
                    CharEvents.GearItemUnequipped?.Invoke(CurrentCharacter.GearData[i]);

                    // Clear the Equipment from the character's gear data
                    CurrentCharacter.GearData[i] = null;

                    // Notify the CharScreen UI to update
                    CharEvents.GearSlotUpdated?.Invoke(null, i);
                }
            }
        }

        // Send the current character to the InventoryScreenController to find gear for empty slots
        void OnGearAutoEquipped()
        {
            CharEvents.CharacterAutoEquipped?.Invoke(CurrentCharacter);
        }

        // Fill in empty slots with gear
        void OnGearAutoEquipped(List<EquipmentSO> gearToEquip)
        {
            if (CurrentCharacter == null)
                return;

            int gearCounter = 0;

            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                if (CurrentCharacter.GearData[i] == null && gearCounter < gearToEquip.Count)
                {
                    CurrentCharacter.GearData[i] = gearToEquip[gearCounter];

                    // notify the CharScreen UI to update
                    CharEvents.GearSlotUpdated?.Invoke(gearToEquip[gearCounter], i);
                    gearCounter++;
                }
            }
        }
    }
}